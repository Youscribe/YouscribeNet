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

        public ProductRequest(Func<HttpClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

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
            {
                this.Errors.Add("Incorrect files uri, need the FileName, ContentType and Uri");
                return null;
            }

            //create product
            using (var client = this.CreateClient())
            {
                var content = this.GetContent(productInformation);
                var productReponse = await client.PostAsync(ApiUrls.ProductUrl, content);

                if (await this.HandleResponseAsync(productReponse, System.Net.HttpStatusCode.Created) == false)
                    return null;

                var product = await this.GetObjectAsync<ProductModel>(productReponse.Content);

                if (await this.uploadFilesAsync(product.Id, filesUri) == false)
                    return null;

                return product;
            }
        }

        private async Task<ProductModel> publishDocumentAsync(ProductModel productInformation, IEnumerable<FileModel> files)
        {
            if (files.Any(f => f.IsValid == false))
            {
                this.Errors.Add("Incorrect files, need the FileName, ContentType and Content");
                return null;
            }
            //create product
            using (var client = this.CreateClient())
            {
                var content = this.GetContent(productInformation);
                var productReponse = await client.PostAsync(ApiUrls.ProductUrl, content);

                if (await this.HandleResponseAsync(productReponse, System.Net.HttpStatusCode.Created) == false)
                    return null;

                var product = await this.GetObjectAsync<ProductModel>(productReponse.Content);

                if (await this.uploadFilesAsync(product.Id, files) == false)
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
                using (var client = this.CreateClient())
                {
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(file.Content), "file", file.FileName);
                    var url = ApiUrls.UploadUrl.Replace("{id}", productId.ToString());
                    var productReponse = await client.PostAsync(url, content);

                    await this.HandleResponseAsync(productReponse, System.Net.HttpStatusCode.OK);
                }
            }

            //finalize
            using (var client = this.CreateClient())
            {
                var url = ApiUrls.ProductEndUploadUrl.Replace("{id}", productId.ToString());
                var response = await client.PutAsync(url, null);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }

        private async Task<bool> uploadFilesAsync(int productId, IEnumerable<Uri> files)
        {
            //upload document files
            foreach (var file in files.Take(nbFilesByDocument))
            {
                using (var client = this.CreateClient())
                {
                    var dico = new Dictionary<string, string>(){
                        {"url", file.ToString()}
                    };
                    var url = ApiUrls.UploadFileUrl.Replace("{id}", productId.ToString());
                    url = url + "?" + dico.ToQueryString();
                    var productReponse = await client.PostAsync(url, null);

                    await this.HandleResponseAsync(productReponse, System.Net.HttpStatusCode.OK);
                }
            }

            //finalize
            using (var client = this.CreateClient())
            {
                var url = ApiUrls.ProductEndUploadUrl.Replace("{id}", productId.ToString());
                var response = await client.PutAsync(url, null);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }

        #endregion

        #region Update

        public async Task<bool> UpdateDocumentAsync(int productId, ProductUpdateModel productInformation)
        {
            var ok = await this.updateDocumentAsync(productId, productInformation);
            if (ok == false)
                return false;
            return await this.finalizeUdateAsync(productId);
        }

        public async Task<bool> UpdateDocumentAsync(int productId, ProductUpdateModel productInformation, IEnumerable<FileModel> files)
        {
            if (files != null && files.Any(f => f.IsValid == false))
            {
                this.Errors.Add("Incorrect files, need the FileName, ContentType and Content");
                return false;
            }
            var ok = await this.updateDocumentAsync(productId, productInformation);
            if (ok == false)
                return false;
            if (files != null)
                return await this.uploadFilesAsync(productId, files);

            return await this.finalizeUdateAsync(productId);
        }

        public async Task<bool> UpdateDocumentAsync(int productId, ProductUpdateModel productInformation, IEnumerable<Uri> filesUri)
        {
            if (filesUri != null && filesUri.Any(c => c.IsValid() == false))
            {
                this.Errors.Add("Incorrect files uri, need the FileName, ContentType and Uri");
                return false;
            }
            var ok = await this.updateDocumentAsync(productId, productInformation);
            if (ok == false)
                return false;

            if (filesUri != null)
                return await this.uploadFilesAsync(productId, filesUri);

            return await this.finalizeUdateAsync(productId);
        }

        private async Task<bool> updateDocumentAsync(int productId, ProductUpdateModel productInformation)
        {
            //update the product
            using (var client = this.CreateClient())
            {
                var url = ApiUrls.ProductUpdateUrl.Replace("{id}", productId.ToString());
                var content = this.GetContent(productInformation);
                var response = await client.PutAsync(url, content);

                return await this.HandleResponseAsync(response, HttpStatusCode.NoContent);
            }
        }

        private async Task<bool> finalizeUdateAsync(int productId)
        {
            using (var client = this.CreateClient())
            {
                var url = ApiUrls.ProductEndUpdateUrl.Replace("{id}", productId.ToString());
                var response = await client.PutAsync(url, null);

                return await this.HandleResponseAsync(response, HttpStatusCode.NoContent);
            }
        }

        #endregion

        #region Thumbnail

        public async Task<bool> UpdateDocumentThumbnailAsync(int productId, Uri imageUri)
        {
            if (imageUri == null || imageUri.IsValid() == false)
            {
                this.Errors.Add("imageUri invalid");
                return false;
            }

            using (var client = this.CreateClient())
            {
                var url = ApiUrls.ThumbnailDataUrl.Replace("{id}", productId.ToString());
                var dico = new Dictionary<string, string>(){
                    {"url", imageUri.ToString()}
                };
                url = url + "?" + dico.ToQueryString();
                var response = await client.PostAsync(url, null);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);
            }
        }

        public async Task<bool> UpdateDocumentThumbnailAsync(int productId, int page)
        {
            using (var client = this.CreateClient())
            {
                var url = ApiUrls.ThumbnailDataUrl.Replace("{id}", productId.ToString());
                var dico = new Dictionary<string, string>(){
                    {"url", page.ToString()}
                };
                var response = await client.PostAsync(url, null);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);
            }
        }

        public async Task<bool> UpdateDocumentThumbnailAsync(int productId, FileModel image)
        {
            if (image.IsValid == false)
            {
                this.Errors.Add("invalid image parameters");
                return false;
            }

            using (var client = this.CreateClient())
            {
                var url = ApiUrls.ThumbnailDataUrl.Replace("{id}", productId.ToString());
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(image.Content), "file", image.FileName);
                var response = await client.PostAsync(url, content);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);
            }
        }

        #endregion

        public async Task<int> GetRightAsync(int productId)
        {
            using (var client = this.CreateClient())
            {
                var url = ApiUrls.ProductRightUrl.Replace("{id}", productId.ToString());
                var response = await client.GetAsync(url);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await this.AddErrorsAsync(response);
                    return -1;
                }
                return await this.GetObjectAsync<int>(response.Content);
            }
        }

        public async Task<Stream> DownloadFileAsync(int productId, string extension)
        {
            var client = this.CreateClient();
            var url = ApiUrls.ProductDownloadByExtensionUrl
                .Replace("{id}", productId.ToString())
                .Replace("{extension}", extension);
            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await this.AddErrorsAsync(response);
                return null;
            }
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> DownloadFileAsync(int productId, int formatTypeId)
        {
            var client = this.CreateClient();
            var url = ApiUrls.ProductDownloadByFormatTypeIdUrl
                .Replace("{id}", productId.ToString())
                .Replace("{formatTypeId}", formatTypeId.ToString());
            var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await this.AddErrorsAsync(response);
                return null;
            }
            return await response.Content.ReadAsStreamAsync();
        }


        protected async Task DownloadFileToStreamAsync(string url, Stream writer, IProgress<DownloadBytesProgress> progressReport)
        {
            int receivedBytes = 0;
            int totalBytes = 0;

            using (var client = this.CreateClient())
            {
                var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await this.AddErrorsAsync(response);
                    return;
                }
                var stream = await response.Content.ReadAsStreamAsync();

                byte[] buffer = new byte[4096];
                totalBytes = (int)response.Content.Headers.ContentLength.Value;

                for (; ; )
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
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
    }
}
