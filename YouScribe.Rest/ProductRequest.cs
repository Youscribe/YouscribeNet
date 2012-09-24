using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using RestSharp;
using YouScribe.Rest.Models;
using YouScribe.Rest.Models.Products;

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

        public ProductModel PublishDocument(ProductModel productInformation, IEnumerable<Uri> filesUri)
        {
            if (filesUri == null || filesUri.Any(c => (c.IsFile || c.IsLoopback || c.IsUnc)))
            {
                this.Errors.Add("Incorrect files uri, need the FileName, ContentType and Uri");
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
                using (MemoryStream ms = new MemoryStream())
                {
                    file.Content.CopyTo(ms);

                    var bytes = ms.ToArray();
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

        private bool uploadFiles(int productId, IEnumerable<Uri> files)
        {
            //upload document files
            foreach (var file in files)
            {
                var request = this.createRequest(ApiUrls.UploadFileUrl, Method.POST)
                    .AddUrlSegment("id", productId.ToString())
                    .AddUrlSegment("url", file.ToString())
                    ;

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
            if (filesUri != null && filesUri.Any(c => (c.IsFile || c.IsLoopback || c.IsUnc)))
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

            using (MemoryStream ms = new MemoryStream())
            {
                image.Content.CopyTo(ms);

                var bytes = ms.ToArray();
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
