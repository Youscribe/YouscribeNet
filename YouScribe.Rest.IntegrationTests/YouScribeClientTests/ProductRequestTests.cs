using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;
using YouScribe.Rest.IntegrationTests.Helpers;

namespace YouScribe.Rest.IntegrationTests.YouScribeClientTests
{
    public class ProductRequestTests
    {
        const string baseUrl = "http://localhost:8080/";

        const string expectedProductResponse = "{\"Id\":42,\"Title\":\"my document title\"}";

        [Fact]
        public void WhenPublishDocumentFromLocalFile_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(baseUrl);

                client.Authorize("test", "password");

                var request = client.CreateProductRequest();

                // Act
                var product = request.PublishDocument(new Models.Products.ProductModel
                {
                    Title = "my document title"
                },
                new[] { 
                    new YouScribe.Rest.Models.FileModel { Content = new MemoryStream(), FileName ="test.pdf", ContentType = "application/pdf" }
                });

                // Assert
                Assert.NotNull(product);
                Assert.Equal(42, product.Id);
                Assert.Equal("my document title", product.Title);
            }
        }

        [Fact]
        public void WhenPublishDocumentFromUrl_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(baseUrl);

                client.Authorize("test", "password");

                var request = client.CreateProductRequest();

                // Act
                var product = request.PublishDocument(new Models.Products.ProductModel
                {
                    Title = "my document title"
                },
                new[] { 
                    new Uri("http://exemple.com/test.pdf")
                });

                // Assert
                Assert.NotNull(product);
                Assert.Equal(42, product.Id);
                Assert.Equal("my document title", product.Title);
            }
        }

        [Fact]
        public void WhenUpdateDocumentFromFile_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(baseUrl);

                client.Authorize("test", "password");

                var request = client.CreateProductRequest();

                // Act
                bool ok = request.UpdateDocument(42, new Models.Products.ProductUpdateModel
                {
                    Description = "ok"
                },
                new[] { 
                    new YouScribe.Rest.Models.FileModel { Content = new MemoryStream(), FileName ="test2.pdf", ContentType = "application/pdf" }
                });

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUpdateDocumentFromUrl_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(baseUrl);

                client.Authorize("test", "password");

                var request = client.CreateProductRequest();

                // Act
                bool ok = request.UpdateDocument(42, new Models.Products.ProductUpdateModel
                {
                    Description = "ok"
                },
                new[] { 
                    new Uri("http://exemple.com/test2.pdf")
                });

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUpdateDocumentThumbnailFromUrl_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(baseUrl);

                client.Authorize("test", "password");

                var request = client.CreateProductRequest();

                // Act
                bool ok = request.UpdateDocumentThumbnail(42, new Uri("http://exemple.com/thumbnail.png"));

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUpdateDocumentThumbnailFromFile_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(baseUrl);

                client.Authorize("test", "password");

                var request = client.CreateProductRequest();

                // Act
                bool ok = request.UpdateDocumentThumbnail(42, new YouScribe.Rest.Models.FileModel
                {
                    Content = new MemoryStream(),
                    FileName = "image.jpg",
                    ContentType = "image/jpg"
                });

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUpdateDocumentThumbnailFromPage_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(baseUrl);

                client.Authorize("test", "password");

                var request = client.CreateProductRequest();

                // Act
                bool ok = request.UpdateDocumentThumbnail(42, 2);

                // Assert
                Assert.True(ok);
            }
        }

        private static void ProductRequestHandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/products":
                    if (context.Request.HttpMethod == "POST")
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                        {
                            context.Response.ContentType = "application/json; charset=utf-8";
                            context.Response.StatusCode = (int)HttpStatusCode.Created;
                            context.Response.OutputStream.Write(expectedProductResponse);
                        }
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    break;
                case "/api/v1/products/42":
                    if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                        context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                    else
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case "/api/v1/upload/42":
                    if (context.Request.HttpMethod == "POST")
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    break;
                case "/api/v1/products/endupload?id=42":
                    if (context.Request.HttpMethod == "PUT")
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    break;
                case "/api/v1/thumbnail/42?url=http%3A%2F%2Fexemple.com%2Fthumbnail.png":
                    if (context.Request.HttpMethod == "PUT")
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    break;
                case "/api/v1/thumbnail/42?page=2":
                    if (context.Request.HttpMethod == "PUT")
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    break;
                case "/api/v1/thumbnail/42":
                    if (context.Request.HttpMethod == "PUT")
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    break;
                case "/api/authorize":
                    context.Response.Headers.Add(ApiUrls.AuthorizeTokenHeaderName, "OK");
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}
