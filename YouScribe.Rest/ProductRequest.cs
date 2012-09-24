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

namespace YouScribe.Rest
{
    class ProductRequest : YouScribeRequest, IProductRequest
    {
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

        public ProductModel PublishDocument(ProductModel productInformation, IEnumerable<FileUrlModel> filesUri)
        {
            var files = filesUri.ToFileModel();
            if (files == null)
            {
                this.Errors.Add("Incorrect files uri, need the FileName, ContentType and Uri");
                return null;
            }
            return this.publishDocument(productInformation, files);
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
            if (productReponse.StatusCode != System.Net.HttpStatusCode.Created)
            {
                this.addErrors(productReponse);
                return null;
            }
            var product = productReponse.Data;

            if (this.uploadFiles(product.Id, files) == false)
                return null;

            return product;
        }

        private bool uploadFiles(int productId, IEnumerable<FileModel> files)
        {
            //upload document files
            foreach (var file in files)
            {
                var request = this.createRequest(ApiUrls.UploadUrl, Method.POST)
                    .AddUrlSegment("id", productId.ToString())
                    ;
                using (var stream = new StreamReader(file.Content))
                {
                    var content = stream.ReadToEnd();
                    var bytes = Encoding.Default.GetBytes(content);
                    request.AddFile("file", bytes, file.FileName, file.ContentType);
                }

                var uploadResponse = this.client.Execute(request);
                if (uploadResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    this.addErrors(uploadResponse);
                    return false;
                }
            }

            //finalize
            var finalizeRequest = this.createRequest(ApiUrls.ProductEndUploadUrl, Method.PUT)
                .AddUrlSegment("id", productId.ToString())
            ;

            var response = this.client.Execute(finalizeRequest);

            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                this.addErrors(response);
                return false;
            }
            return true;
        }

        #endregion

        #region Update

        public bool UpdateDocument(int productId, ProductUpdateModel productInformation)
        {
            return this.updateDocument(productId, productInformation, null);
        }

        public bool UpdateDocument(int productId, ProductUpdateModel productInformation, IEnumerable<FileModel> files)
        {
            return this.updateDocument(productId, productInformation, files);
        }

        public bool UpdateDocument(int productId, ProductUpdateModel productInformation, IEnumerable<FileUrlModel> filesUri)
        {
            var files = filesUri.ToFileModel();
            if (files == null)
            {
                this.Errors.Add("Incorrect files uri, need the FileName, ContentType and Uri");
                return false;
            }
            return this.updateDocument(productId, productInformation, files);
        }

        private bool updateDocument(int productId, ProductUpdateModel productInformation, IEnumerable<FileModel> files)
        {
            if (files != null && files.Any(f => f.IsValid == false))
            {
                this.Errors.Add("Incorrect files, need the FileName, ContentType and Content");
                return false;
            }
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
            if (files != null && files.Any())
            {
                if (this.uploadFiles(productId, files) == false)
                    return false;
            }
            else
            {
                //finalize the update
                request = this.createRequest(ApiUrls.ProductEndUpdateUrl, Method.PUT)
                    .AddUrlSegment("id", productId.ToString())
                ;

                response = this.client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    this.addErrors(response);
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Thumbnail

        public bool UpdateDocumentThumbnail(int productId, Uri imageUri)
        {
            if (imageUri == null || imageUri.IsFile || imageUri.IsLoopback || imageUri.IsUnc)
            {
                this.Errors.Add("imageUri invalid");
                return false;
            }
            var request = this.createRequest(ApiUrls.ThumbnailLinkUrl, Method.POST)
                .AddUrlSegment("id", productId.ToString())
                .AddUrlSegment("url", imageUri.ToString())
                ;
            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                this.addErrors(response);
                return false;
            }
            return true;
        }

        public bool UpdateDocumentThumbnail(int productId, int page)
        {
            var request = this.createRequest(ApiUrls.ThumbnailPageUrl, Method.POST)
                .AddUrlSegment("id", productId.ToString())
                .AddUrlSegment("page", page.ToString())
                ;

            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                this.addErrors(response);
                return false;
            }
            return true;
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

            using (var stream = new StreamReader(image.Content))
            {
                var content = stream.ReadToEnd();
                var bytes = Encoding.Default.GetBytes(content);
                request.AddFile("file", bytes, image.FileName, image.ContentType);
            }

            var response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                this.addErrors(response);
                return false;
            }
            return true;
        }

        #endregion
    }
}
