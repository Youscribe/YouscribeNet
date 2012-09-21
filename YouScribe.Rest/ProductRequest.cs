using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public ProductModel PublishDocument(ProductModel productInformation, IEnumerable<FileModel> files)
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
            //upload document
            foreach (var file in files)
            {
                request = this.createRequest(ApiUrls.UploadUrl, Method.POST)
                    .AddUrlSegment("id", product.Id.ToString())
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
                    this.addErrors(productReponse);
                    return null;
                }
            }

            //finalize
            request = this.createRequest(ApiUrls.ProductEndUploadUrl, Method.PUT)
                .AddUrlSegment("id", product.Id.ToString())
            ;

            var response = this.client.Execute(request);

            if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                this.addErrors(productReponse);
                return null;
            }

            return product;
        }

        public ProductModel PublishDocument(ProductModel productInformation, IEnumerable<Uri> filesUri)
        {
            throw new NotImplementedException();
        }

        public void UpdateDocument(int documentId, ProductUpdateModel productInformation)
        {
            throw new NotImplementedException();
        }

        public void UpdateDocument(int documentId, ProductUpdateModel productInformation, IEnumerable<FileModel> files)
        {
            throw new NotImplementedException();
        }

        public void UpdateDocument(int documentId, ProductUpdateModel productInformation, IEnumerable<Uri> filesUri)
        {
            throw new NotImplementedException();
        }

        public void UpdateDocumentThumbnail(int documentId, Uri imageUri)
        {
            throw new NotImplementedException();
        }

        public void UpdateDocumentThumbnail(int documentId, int page)
        {
            throw new NotImplementedException();
        }

        public void UpdateDocumentThumbnail(int documentId, FileModel image)
        {
            throw new NotImplementedException();
        }
    }
}
