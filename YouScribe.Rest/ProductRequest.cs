using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using YouScribe.Rest.Models;
using YouScribe.Rest.Models.Products;
using YouScribe.Rest.Helpers;
using System.Threading.Tasks;
using System.Net.Http;

namespace YouScribe.Rest
{
    class ProductRequest : YouScribeRequest, IProductRequest
    {
        const int nbFilesByDocument = 3;

        public ProductRequest(Func<DisposableClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

        private async Task<ProductGetModel> GetAsync(string url, int id)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                url = url.Replace("{id}", id.ToString());
                var response = await client.GetAsync(this.GetUri(url)).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    await this.AddErrorsAsync(response).ConfigureAwait(false);
                    return null;
                }
                return await this.GetObjectAsync<ProductGetModel>(response.Content).ConfigureAwait(false);
            }
        }

        public Task<ProductGetModel> GetAsync(int id)
        {
            return this.GetAsync(ApiUrls.ProductGetUrl, id);
        }

        public Task<ProductGetModel> GetAsyncV2(int id)
        {
            return this.GetAsync(ApiUrls.ProductGetUrlV2, id);
        }


        private async Task<IEnumerable<ProductGetModel>> GetAsync(string url, IEnumerable<int> ids)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var content = this.GetContent(ids);
                var response = await client.PostAsync(this.GetUri(url), content).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    await this.AddErrorsAsync(response).ConfigureAwait(false);
                    return null;
                }
                return await this.GetObjectAsync<IEnumerable<ProductGetModel>>(response.Content).ConfigureAwait(false);
            }
        }

        public Task<IEnumerable<ProductGetModel>> GetAsync(IEnumerable<int> ids)
        {
            return this.GetAsync(ApiUrls.ProductUrlByIds, ids);
        }

        public Task<IEnumerable<ProductGetModel>> GetAsyncV2(IEnumerable<int> ids)
        {
            return this.GetAsync(ApiUrls.ProductUrlByIdsV2, ids);
        }

        #region PublishDocument

        public Task<ProductModel> PublishDocumentAsync(ProductModel productInformation, IEnumerable<FileModel> files)
        {
            if (files == null || files.Any() == false)
                throw new ArgumentNullException("files", "You need to select file(s) to upload");
            return this.publishDocumentAsync(productInformation, files);
        }

        public async Task<ProductModel> PublishDocumentAsync(ProductModel productInformation, IEnumerable<Uri> filesUri)
        {
            if (filesUri == null || filesUri.Any(c => (c.IsValid() == false)))
                throw new ArgumentException("Incorrect files uri, need the FileName, ContentType and Uri", "filesUri");

            //create product
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var content = this.GetContent(productInformation);
                var productReponse = await client.PostAsync(this.GetUri(ApiUrls.ProductUrl), content).ConfigureAwait(false);

                if (await this.HandleResponseAsync(productReponse, System.Net.HttpStatusCode.Created).ConfigureAwait(false) == false)
                    return null;

                var product = await this.GetObjectAsync<ProductModel>(productReponse.Content).ConfigureAwait(false);

                if (await this.uploadFilesAsync(product.Id, filesUri).ConfigureAwait(false) == false)
                    return null;

                return product;
            }
        }

        private async Task<ProductModel> publishDocumentAsync(ProductModel productInformation, IEnumerable<FileModel> files)
        {
            if (files.Any(f => f.IsValid == false))
                throw new ArgumentException("Incorrect files, need the FileName, ContentType and Content", "files");
            //create product
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var content = this.GetContent(productInformation);
                var productReponse = await client.PostAsync(this.GetUri(ApiUrls.ProductUrl), content).ConfigureAwait(false);

                if (await this.HandleResponseAsync(productReponse, System.Net.HttpStatusCode.Created).ConfigureAwait(false) == false)
                    return null;

                var product = await this.GetObjectAsync<ProductModel>(productReponse.Content).ConfigureAwait(false);

                if (await this.uploadFilesAsync(product.Id, files).ConfigureAwait(false) == false)
                    return null;

                return product;
            }
        }

        private async Task<bool> uploadFilesAsync(int productId, IEnumerable<FileModel> files)
        {
            //select on file by content type and limit to nbFilesByDocument
            files = files.GroupBy(c => c.ContentType)
                .Select(c => c.First())
                .Take(nbFilesByDocument)
                .ToList();

            //upload document files
            foreach (var file in files)
            {
                using (var fileClient = this.CreateClient())
                { 
                    var client = fileClient.Client;
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(file.Content), "file", file.FileName);
                    var fileUrl = ApiUrls.UploadUrl.Replace("{id}", productId.ToString());
                    var productReponse = await client.PostAsync(this.GetUri(fileUrl), content).ConfigureAwait(false);

                    await this.HandleResponseAsync(productReponse, System.Net.HttpStatusCode.OK).ConfigureAwait(false);
                }
            }

            //finalize
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.ProductEndUploadUrl.Replace("{id}", productId.ToString());
                var response = await client.PutAsync(this.GetUri(url), null).ConfigureAwait(false);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
            }
        }

        private async Task<bool> uploadFilesAsync(int productId, IEnumerable<Uri> files)
        {
            //upload document files
            foreach (var file in files.Take(nbFilesByDocument))
            {
                using (var fileClient = this.CreateClient())
                {
                    var client = fileClient.Client;
                    var dico = new Dictionary<string, string>(){
                        {"url", file.ToString()}
                    };
                    var fileUrl = ApiUrls.UploadFileUrl.Replace("{id}", productId.ToString());
                    fileUrl = fileUrl + "?" + dico.ToQueryString();
                    var productReponse = await client.PostAsync(this.GetUri(fileUrl), null).ConfigureAwait(false);

                    await this.HandleResponseAsync(productReponse, System.Net.HttpStatusCode.OK).ConfigureAwait(false);
                }
            }

            //finalize
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.ProductEndUploadUrl.Replace("{id}", productId.ToString());
                var response = await client.PutAsync(this.GetUri(url), null).ConfigureAwait(false);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
            }
        }

        #endregion

        #region Update

        public async Task<bool> UpdateDocumentAsync(int productId, ProductUpdateModel productInformation)
        {
            var ok = await this.updateDocumentAsync(productId, productInformation).ConfigureAwait(false);
            if (ok == false)
                return false;
            return await this.finalizeUdateAsync(productId).ConfigureAwait(false);
        }

        public async Task<bool> UpdateDocumentAsync(int productId, ProductUpdateModel productInformation, IEnumerable<FileModel> files)
        {
            if (files != null && files.Any(f => f.IsValid == false))
                throw new ArgumentException("Incorrect files, need the FileName, ContentType and Content", "files");
            var ok = await this.updateDocumentAsync(productId, productInformation).ConfigureAwait(false);
            if (ok == false)
                return false;
            if (files != null)
                return await this.uploadFilesAsync(productId, files).ConfigureAwait(false);

            return await this.finalizeUdateAsync(productId).ConfigureAwait(false);
        }

        public async Task<bool> UpdateDocumentAsync(int productId, ProductUpdateModel productInformation, IEnumerable<Uri> filesUri)
        {
            if (filesUri != null && filesUri.Any(c => c.IsValid() == false))
                throw new ArgumentException("Incorrect files uri, need the FileName, ContentType and Uri", "filesUri");
            var ok = await this.updateDocumentAsync(productId, productInformation).ConfigureAwait(false);
            if (ok == false)
                return false;

            if (filesUri != null)
                return await this.uploadFilesAsync(productId, filesUri).ConfigureAwait(false);

            return await this.finalizeUdateAsync(productId).ConfigureAwait(false);
        }

        private async Task<bool> updateDocumentAsync(int productId, ProductUpdateModel productInformation)
        {
            //update the product
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.ProductUpdateUrl.Replace("{id}", productId.ToString());
                var content = this.GetContent(productInformation);
                var response = await client.PutAsync(this.GetUri(url), content).ConfigureAwait(false);

                return await this.HandleResponseAsync(response, HttpStatusCode.NoContent).ConfigureAwait(false);
            }
        }

        private async Task<bool> finalizeUdateAsync(int productId)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.ProductEndUpdateUrl.Replace("{id}", productId.ToString());
                var response = await client.PutAsync(this.GetUri(url), null).ConfigureAwait(false);

                return await this.HandleResponseAsync(response, HttpStatusCode.NoContent).ConfigureAwait(false);
            }
        }

        #endregion

        #region Thumbnail

        public async Task<bool> UpdateDocumentThumbnailAsync(int productId, Uri imageUri)
        {
            if (imageUri == null || imageUri.IsValid() == false)
                throw new ArgumentException("ImageUri invalid", "imageUri");

            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.ThumbnailDataUrl.Replace("{id}", productId.ToString());
                var dico = new Dictionary<string, string>(){
                {"url", imageUri.ToString()}
            };
                url = url + "?" + dico.ToQueryString();
                var response = await client.PostAsync(this.GetUri(url), null).ConfigureAwait(false);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK).ConfigureAwait(false);
            }
        }

        public async Task<bool> UpdateDocumentThumbnailAsync(int productId, int page)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.ThumbnailDataUrl.Replace("{id}", productId.ToString());
                var dico = new Dictionary<string, string>(){
                {"url", page.ToString()}
            };
                var response = await client.PostAsync(this.GetUri(url), null).ConfigureAwait(false);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK).ConfigureAwait(false);
            }
        }

        public async Task<bool> UpdateDocumentThumbnailAsync(int productId, FileModel image)
        {
            if (image.IsValid == false)
                throw new ArgumentException("Invalid image parameters", "image");

            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.ThumbnailDataUrl.Replace("{id}", productId.ToString());
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(image.Content), "file", image.FileName);
                var response = await client.PostAsync(this.GetUri(url), content).ConfigureAwait(false);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK).ConfigureAwait(false);
            }
        }

        #endregion

        public async Task<int> GetRightAsync(int productId)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.ProductRightUrl.Replace("{id}", productId.ToString());
                var response = await client.GetAsync(this.GetUri(url)).ConfigureAwait(false);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await this.AddErrorsAsync(response).ConfigureAwait(false);
                    return -1;
                }
                return await this.GetObjectAsync<int>(response.Content).ConfigureAwait(false);
            }
        }

        public async Task<Stream> DownloadFileAsync(int productId, string extension)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.ProductDownloadByExtensionUrl
                    .Replace("{id}", productId.ToString())
                    .Replace("{extension}", extension);
                var response = await client.GetAsync(this.GetUri(url), HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await this.AddErrorsAsync(response).ConfigureAwait(false);
                    return null;
                }
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
        }

        public async Task<Stream> DownloadFileAsync(int productId, int formatTypeId)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.ProductDownloadByFormatTypeIdUrl
                    .Replace("{id}", productId.ToString())
                    .Replace("{formatTypeId}", formatTypeId.ToString());
                var response = await client.GetAsync(this.GetUri(url), HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await this.AddErrorsAsync(response).ConfigureAwait(false);
                    return null;
                }
                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
        }


        protected async Task DownloadFileToStreamAsync(string url, Stream writer, IProgress<DownloadBytesProgress> progressReport)
        {
            int receivedBytes = 0;
            int totalBytes = 0;

            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var response = await client.GetAsync(this.GetUri(url), HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await this.AddErrorsAsync(response).ConfigureAwait(false);
                    return;
                }
                var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                byte[] buffer = new byte[4096];
                totalBytes = (int)response.Content.Headers.ContentLength.Value;

                for (; ; )
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    if (bytesRead == 0)
                        break;

                    writer.Write(buffer, 0, bytesRead);
                    receivedBytes += bytesRead;
                    if (progressReport != null)
                    {
                        var args = new DownloadBytesProgress(url, receivedBytes, totalBytes);
                        progressReport.Report(args);
                    }
                }
            }
        }

        public Task DownloadFileToStreamAsync(int productId, int formatTypeId, Stream writer, IProgress<DownloadBytesProgress> progressReport)
        {
            var urlToDownload = ApiUrls.ProductDownloadByFormatTypeIdUrl
                .Replace("{id}", productId.ToString())
                .Replace("{formatTypeId}", formatTypeId.ToString());
            return this.DownloadFileToStreamAsync(urlToDownload, writer, progressReport);
        }

        public Task DownloadFileToStreamAsync(int productId, string extension, Stream writer, IProgress<DownloadBytesProgress> progressReport)
        {
			var urlToDownload = ApiUrls.ProductDownloadByExtensionUrl
                .Replace("{id}", productId.ToString())
                .Replace("{extension}", extension);
            return this.DownloadFileToStreamAsync(urlToDownload, writer, progressReport);
        }

        public async Task<IEnumerable<ProductUrlsModel>> GetProductUrlsAsync(IEnumerable<int> ids)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.ProductGetUrlsByIds;
                var content = this.GetContent(ids);
                var response = await client.PostAsync(this.GetUri(url), content).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    await this.AddErrorsAsync(response).ConfigureAwait(false);
                    return null;
                }
                return await this.GetObjectAsync<IEnumerable<ProductUrlsModel>>(response.Content).ConfigureAwait(false);
            }
        }
    }
}
