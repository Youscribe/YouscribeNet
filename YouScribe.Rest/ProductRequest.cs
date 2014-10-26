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
            using (var client = this.clientFactory())
            {
                var productReponse = await client.PostAsync(ApiUrls.ProductUrl, productInformation);

                if (await this.HandleResponseAsync(productReponse, System.Net.HttpStatusCode.Created) == false)
                    return null;

                var product = await productReponse.Content.ReadAsync<ProductModel>();

                if (await this.uploadFilesAsync(product.Id, filesUri) == false)
                    return null;

                return product;
            }
        }

        private Task<ProductModel> publishDocumentAsync(ProductModel productInformation, IEnumerable<FileModel> files)
        {
            if (files.Any(f => f.IsValid == false))
            {
                this.Errors.Add("Incorrect files, need the FileName, ContentType and Content");
                return null;
            }
            //create product
            var request = this.createRequest(ApiUrls.ProductUrl, Method.POST);

            request.AddBody(productInformation);

            var productReponse = this.client.Execute<ProductModel>(request);
            if (this.handleResponse(productReponse, System.Net.HttpStatusCode.Created) == false)
                return null;

            var product = productReponse.Data;

            if (this.uploadFiles(product.Id, files) == false)
                return null;

            return product;
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
                var request = this.createRequest(ApiUrls.UploadUrl, Method.POST)
                    .AddUrlSegment("id", productId.ToString())
                    ;
                using (MemoryStream ms = new MemoryStream())
                {
                    file.Content.CopyTo(ms);

                    var bytes = ms.ToArray();
                    request.AddFile("file", bytes, file.FileName, file.ContentType);
                }

                var uploadResponse = this.client.Execute(request);
                await this.HandleResponseAsync(uploadResponse, System.Net.HttpStatusCode.OK);
            }

            //finalize
            var finalizeRequest = this.createRequest(ApiUrls.ProductEndUploadUrl, Method.PUT)
                .AddUrlSegment("id", productId.ToString())
            ;

            var response = this.client.Execute(finalizeRequest);

            if (await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent) == false)
                return false;

            return true;
        }

        private async Task<bool> uploadFilesAsync(int productId, IEnumerable<Uri> files)
        {
            //upload document files
            foreach (var file in files.Take(nbFilesByDocument))
            {
                var request = this.createRequest(ApiUrls.UploadFileUrl, Method.POST)
                    .AddUrlSegment("id", productId.ToString())
                    .AddUrlSegment("url", file.ToString())
                    ;

                var uploadResponse = this.client.Execute(request);
                await this.HandleResponseAsync(uploadResponse, System.Net.HttpStatusCode.OK);
            }

            //finalize
            var finalizeRequest = this.createRequest(ApiUrls.ProductEndUploadUrl, Method.PUT)
                .AddUrlSegment("id", productId.ToString())
            ;

            var response = this.client.Execute(finalizeRequest);

            if (await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent) == false)
                return false;
            
            return true;
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
            var request = this.createRequest(ApiUrls.ProductUpdateUrl, Method.PUT)
                .AddUrlSegment("id", productId.ToString())
                ;
            request.AddBody(productInformation);

            var response = this.client.Execute(request);
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                await this.AddErrorsAsync(response);
                return false;
            }
            return true;
        }

        private async Task<bool> finalizeUdateAsync(int productId)
        {
            var request = this.createRequest(ApiUrls.ProductEndUpdateUrl, Method.PUT)
                   .AddUrlSegment("id", productId.ToString())
               ;

            var response = this.client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                await this.AddErrorsAsync(response);
                return false;
            }
            return true;
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
            var request = this.createRequest(ApiUrls.ThumbnailLinkUrl, Method.POST)
                .AddUrlSegment("id", productId.ToString())
                .AddUrlSegment("url", imageUri.ToString())
                ;
            var response = client.Execute(request);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);
        }

        public async Task<bool> UpdateDocumentThumbnailAsync(int productId, int page)
        {
            var request = this.createRequest(ApiUrls.ThumbnailPageUrl, Method.POST)
                .AddUrlSegment("id", productId.ToString())
                .AddUrlSegment("page", page.ToString())
                ;

            var response = client.Execute(request);
            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);
        }

        public async Task<bool> UpdateDocumentThumbnailAsync(int productId, FileModel image)
        {
            if (image.IsValid == false)
            {
                this.Errors.Add("invalid image parameters");
                return false;
            }
            var request = this.createRequest(ApiUrls.ThumbnailDataUrl, Method.POST)
                .AddUrlSegment("id", productId.ToString());

            using (MemoryStream ms = new MemoryStream())
            {
                image.Content.CopyTo(ms);

                var bytes = ms.ToArray();
                request.AddFile("file", bytes, image.FileName, image.ContentType);
            }

            var response = client.Execute(request);
            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);
        }

        #endregion

        public async Task<int> GetRightAsync(int productId)
        {
            var request = this.createRequest(ApiUrls.ProductRightUrl, Method.GET)
                .AddUrlSegment("id", productId.ToString())
                ;
            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await this.AddErrorsAsync(response);
                return -1;
            }
            return int.Parse(response.Content);
        }

        public async Task<Stream> DownloadFileAsync(int productId, string extension)
        {
            var request = this.createRequest(ApiUrls.ProductDownloadByExtensionUrl, Method.GET)
                .AddUrlSegment("id", productId.ToString())
                .AddUrlSegment("extension", extension)
                ;
            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await this.AddErrorsAsync(response);
                return null;
            }
            return new MemoryStream(response.RawBytes);
        }

        public async Task<Stream> DownloadFileAsync(int productId, int formatTypeId)
        {
            var request = this.createRequest(ApiUrls.ProductDownloadByFormatTypeIdUrl, Method.GET)
                .AddUrlSegment("id", productId.ToString())
                .AddUrlSegment("formatTypeId", formatTypeId.ToString())
                ;
            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await this.AddErrorsAsync(response);
                return null;
            }
            return new MemoryStream(response.RawBytes);
        }

#if __ANDROID__
        protected async Task DownloadFileToPathAsync(string url, string path, IProgress<DownloadBytesProgress> progressReport)
        {
            int receivedBytes = 0;
            int totalBytes = 0;
            WebClient client = new WebClient();

            client.Headers.Add(ApiUrls.AuthorizeTokenHeaderName, this.authorizeToken);
			try
			{
				using (var stream = await client.OpenReadTaskAsync(url))
	            {
					using (var writer = File.OpenWrite(path))
					{
		                byte[] buffer = new byte[4096];
		                totalBytes = Int32.Parse(client.ResponseHeaders[HttpResponseHeader.ContentLength]);

		                for (; ; )
		                {
		                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
		                    if (bytesRead == 0)
		                    {
		                        await Task.Yield();
		                        break;
		                    }

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
			}
			catch (Exception e) {
				this.Errors.Add (e.Message);
			}
        }

        public Task DownloadFileToPathAsync(int productId, int formatTypeId, string path, IProgress<DownloadBytesProgress> progressReport)
        {
            var urlToDownload = ApiUrls.ProductDownloadByFormatTypeIdUrl
                .Replace("{id}", productId.ToString())
                .Replace("{formatTypeId}", formatTypeId.ToString());
			return this.DownloadFileToPathAsync(client.BaseUrl.TrimEnd('/') + "/" + urlToDownload, path, progressReport);
        }

        public Task DownloadFileToPathAsync(int productId, string extension, string path, IProgress<DownloadBytesProgress> progressReport)
        {
			var urlToDownload = ApiUrls.ProductDownloadByExtensionUrl
                .Replace("{id}", productId.ToString())
                .Replace("{extension}", extension);
			return this.DownloadFileToPathAsync(client.BaseUrl.TrimEnd('/') + "/" + urlToDownload, path, progressReport);
        }
#else
        public void DownloadFileToPath(int productId, int formatTypeId, string path)
        {
            var request = this.createRequest(ApiUrls.ProductDownloadByFormatTypeIdUrl, Method.GET)
                .AddUrlSegment("id", productId.ToString())
                .AddUrlSegment("formatTypeId", formatTypeId.ToString())
                ;
            using (var writer = File.OpenWrite(path))
            {
                request.ResponseWriter = (responseStream) => responseStream.CopyTo(writer);
                var response = client.Execute(request);
            }
        }

        public void DownloadFileToPath(int productId, string extension, string path)
        {
            var request = this.createRequest(ApiUrls.ProductDownloadByExtensionUrl, Method.GET)
                .AddUrlSegment("id", productId.ToString())
                .AddUrlSegment("extension", extension)
                ;
            using (var writer = File.OpenWrite(path))
            {
                request.ResponseWriter = (responseStream) => responseStream.CopyTo(writer);
                var response = client.Execute(request);
            }
        }
#endif
    }
}
