using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using RestSharp;
using YouScribe.Rest.Models;
using YouScribe.Rest.Models.Products;
using YouScribe.Rest.Helpers;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    class ProductRequest : YouScribeRequest, IProductRequest
    {
        const int nbFilesByDocument = 3;

        public ProductRequest(IRestClient client, string authorizeToken)
            : base(client, authorizeToken)
        { }

        #region PublishDocument

        public ProductModel PublishDocument(ProductModel productInformation, IEnumerable<FileModel> files)
        {
            if (files == null || files.Any() == false)
                throw new ArgumentNullException("files", "You need to select file(s) to upload");
            return this.publishDocument(productInformation, files);
        }

        public ProductModel PublishDocument(ProductModel productInformation, IEnumerable<Uri> filesUri)
        {
            if (filesUri == null || filesUri.Any(c => (c.IsValid() == false)))
            {
                this.Errors.Add("Incorrect files uri, need the FileName, ContentType and Uri");
                return null;
            }

            //create product
            var request = this.createRequest(ApiUrls.ProductUrl, Method.POST);

            request.AddBody(productInformation);

            var productReponse = this.client.Execute<ProductModel>(request);

            if (this.handleResponse(productReponse, System.Net.HttpStatusCode.Created) == false)
                return null;

            var product = productReponse.Data;

            if (this.uploadFiles(product.Id, filesUri) == false)
                return null;

            return product;
        }

        private ProductModel publishDocument(ProductModel productInformation, IEnumerable<FileModel> files)
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

        private bool uploadFiles(int productId, IEnumerable<FileModel> files)
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
                this.handleResponse(uploadResponse, System.Net.HttpStatusCode.OK);
            }

            //finalize
            var finalizeRequest = this.createRequest(ApiUrls.ProductEndUploadUrl, Method.PUT)
                .AddUrlSegment("id", productId.ToString())
            ;

            var response = this.client.Execute(finalizeRequest);

            if (this.handleResponse(response, System.Net.HttpStatusCode.NoContent) == false)
                return false;

            return true;
        }

        private bool uploadFiles(int productId, IEnumerable<Uri> files)
        {
            //upload document files
            foreach (var file in files.Take(nbFilesByDocument))
            {
                var request = this.createRequest(ApiUrls.UploadFileUrl, Method.POST)
                    .AddUrlSegment("id", productId.ToString())
                    .AddUrlSegment("url", file.ToString())
                    ;

                var uploadResponse = this.client.Execute(request);
                this.handleResponse(uploadResponse, System.Net.HttpStatusCode.OK);
            }

            //finalize
            var finalizeRequest = this.createRequest(ApiUrls.ProductEndUploadUrl, Method.PUT)
                .AddUrlSegment("id", productId.ToString())
            ;

            var response = this.client.Execute(finalizeRequest);

            if (this.handleResponse(response, System.Net.HttpStatusCode.NoContent) == false)
                return false;
            
            return true;
        }

        #endregion

        #region Update

        public bool UpdateDocument(int productId, ProductUpdateModel productInformation)
        {
            var ok = this.updateDocument(productId, productInformation);
            if (ok == false)
                return false;
            return this.finalizeUdate(productId);
        }

        public bool UpdateDocument(int productId, ProductUpdateModel productInformation, IEnumerable<FileModel> files)
        {
            if (files != null && files.Any(f => f.IsValid == false))
            {
                this.Errors.Add("Incorrect files, need the FileName, ContentType and Content");
                return false;
            }
            var ok = this.updateDocument(productId, productInformation);
            if (ok == false)
                return false;
            if (files != null)
                return this.uploadFiles(productId, files);

            return this.finalizeUdate(productId);
        }

        public bool UpdateDocument(int productId, ProductUpdateModel productInformation, IEnumerable<Uri> filesUri)
        {
            if (filesUri != null && filesUri.Any(c => c.IsValid() == false))
            {
                this.Errors.Add("Incorrect files uri, need the FileName, ContentType and Uri");
                return false;
            }
            var ok = this.updateDocument(productId, productInformation);
            if (ok == false)
                return false;

            if (filesUri != null)
                return this.uploadFiles(productId, filesUri);

            return this.finalizeUdate(productId);
        }

        private bool updateDocument(int productId, ProductUpdateModel productInformation)
        {
            //update the product
            var request = this.createRequest(ApiUrls.ProductUpdateUrl, Method.PUT)
                .AddUrlSegment("id", productId.ToString())
                ;
            request.AddBody(productInformation);

            var response = this.client.Execute(request);
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                this.addErrors(response);
                return false;
            }
            return true;
        }

        private bool finalizeUdate(int productId)
        {
            var request = this.createRequest(ApiUrls.ProductEndUpdateUrl, Method.PUT)
                   .AddUrlSegment("id", productId.ToString())
               ;

            var response = this.client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                this.addErrors(response);
                return false;
            }
            return true;
        }

        #endregion

        #region Thumbnail

        public bool UpdateDocumentThumbnail(int productId, Uri imageUri)
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

            return this.handleResponse(response, System.Net.HttpStatusCode.OK);
        }

        public bool UpdateDocumentThumbnail(int productId, int page)
        {
            var request = this.createRequest(ApiUrls.ThumbnailPageUrl, Method.POST)
                .AddUrlSegment("id", productId.ToString())
                .AddUrlSegment("page", page.ToString())
                ;

            var response = client.Execute(request);
            return this.handleResponse(response, System.Net.HttpStatusCode.OK);
        }

        public bool UpdateDocumentThumbnail(int productId, FileModel image)
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
            return this.handleResponse(response, System.Net.HttpStatusCode.OK);
        }

        #endregion

        public int GetRight(int productId)
        {
            var request = this.createRequest(ApiUrls.ProductRightUrl, Method.GET)
                .AddUrlSegment("id", productId.ToString())
                ;
            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                this.addErrors(response);
                return -1;
            }
            return int.Parse(response.Content);
        }

        public Stream DownloadFile(int productId, string extension)
        {
            var request = this.createRequest(ApiUrls.ProductDownloadByExtensionUrl, Method.GET)
                .AddUrlSegment("id", productId.ToString())
                .AddUrlSegment("extension", extension)
                ;
            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                this.addErrors(response);
                return null;
            }
            return new MemoryStream(response.RawBytes);
        }

        public Stream DownloadFile(int productId, int formatTypeId)
        {
            var request = this.createRequest(ApiUrls.ProductDownloadByFormatTypeIdUrl, Method.GET)
                .AddUrlSegment("id", productId.ToString())
                .AddUrlSegment("formatTypeId", formatTypeId.ToString())
                ;
            var response = client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                this.addErrors(response);
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
            using (var stream = await client.OpenReadTaskAsync(url))
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

                    receivedBytes += bytesRead;
                    if (progressReport != null)
                    {
                        DownloadBytesProgress args = new DownloadBytesProgress(url, receivedBytes, totalBytes);
                        progressReport.Report(args);
                    }
                }
            }
        }

        public Task DownloadFileToPathAsync(int productId, int formatTypeId, string path, IProgress<DownloadBytesProgress> progressReport)
        {
            var urlToDownload = ApiUrls.ProductDownloadByFormatTypeIdUrl
                .Replace("{id}", productId.ToString())
                .Replace("{formatTypeId}", formatTypeId.ToString());
            return this.DownloadFileToPathAsync(urlToDownload, path, progressReport);
        }

        public Task DownloadFileToPathAsync(int productId, string extension, string path, IProgress<DownloadBytesProgress> progressReport)
        {
            var urlToDownload = ApiUrls.ProductDownloadByFormatTypeIdUrl
                .Replace("{id}", productId.ToString())
                .Replace("{extension}", extension);
            return this.DownloadFileToPathAsync(urlToDownload, path, progressReport);
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
