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

        static string requestContent = null;

        [Fact]
        public void WhenPublishDocumentFromLocalFile_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateProductRequest();

                // Act
                var product = request.PublishDocumentAsync(new Models.Products.ProductModel
                {
                    Title = "my document title"
                },
                new[] { 
                    new YouScribe.Rest.Models.FileModel { Content = new MemoryStream(), FileName ="test.pdf", ContentType = "application/pdf" }
                }).Result;

                // Assert
                Assert.NotNull(product);
                Assert.Equal(42, product.Id);
                Assert.Equal("my document title", product.Title);
                Assert.Equal("{\"Id\":0,\"Title\":\"my document title\",\"Description\":null,\"Collection\":null,\"PublishDate\":null,\"EAN13\":null,\"Public\":true,\"IsFree\":true,\"Price\":null,\"People\":null,\"Languages\":null,\"Tags\":null,\"CategoryId\":0,\"ThemeId\":0,\"AllowPasteAndCut\":false,\"AllowPrint\":false,\"AllowPrintOnDemand\":false,\"AllowDownload\":true,\"AllowStreaming\":true,\"IsAdultContent\":false,\"PreviewNbPage\":null,\"PreviewPercentPage\":null,\"PreviewRange\":null,\"CopyrightInformation\":0,\"LicenceName\":null}", 
                    requestContent);
            }
        }

        [Fact]
        public void WhenPublishDocumentFromUrl_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateProductRequest();

                // Act
                var product = request.PublishDocumentAsync(new Models.Products.ProductModel
                {
                    Title = "my document title"
                },
                new[] { 
                    new Uri("http://exemple.com/test.pdf")
                }).Result;

                // Assert
                Assert.NotNull(product);
                Assert.Equal(42, product.Id);
                Assert.Equal("my document title", product.Title);
                Assert.Equal("{\"Id\":0,\"Title\":\"my document title\",\"Description\":null,\"Collection\":null,\"PublishDate\":null,\"EAN13\":null,\"Public\":true,\"IsFree\":true,\"Price\":null,\"People\":null,\"Languages\":null,\"Tags\":null,\"CategoryId\":0,\"ThemeId\":0,\"AllowPasteAndCut\":false,\"AllowPrint\":false,\"AllowPrintOnDemand\":false,\"AllowDownload\":true,\"AllowStreaming\":true,\"IsAdultContent\":false,\"PreviewNbPage\":null,\"PreviewPercentPage\":null,\"PreviewRange\":null,\"CopyrightInformation\":0,\"LicenceName\":null}", 
                    requestContent);
            }
        }

        [Fact]
        public void WhenUpdateDocumentFromFile_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateProductRequest();

                // Act
                bool ok = request.UpdateDocumentAsync(42, new Models.Products.ProductUpdateModel
                {
                    Description = "ok"
                },
                new[] { 
                    new YouScribe.Rest.Models.FileModel { Content = new MemoryStream(), FileName ="test2.pdf", ContentType = "application/pdf" }
                }).Result;

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUpdateDocumentFromUrl_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateProductRequest();

                // Act
                bool ok = request.UpdateDocumentAsync(42, new Models.Products.ProductUpdateModel
                {
                    Description = "ok"
                },
                new[] { 
                    new Uri("http://exemple.com/test2.pdf")
                }).Result;

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUpdateDocumentThumbnailFromUrl_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateProductRequest();

                // Act
                bool ok = request.UpdateDocumentThumbnailAsync(42, new Uri("http://exemple.com/thumbnail.png")).Result;

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUpdateDocumentThumbnailFromFile_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateProductRequest();

                // Act
                bool ok = request.UpdateDocumentThumbnailAsync(42, new YouScribe.Rest.Models.FileModel
                {
                    Content = new MemoryStream(),
                    FileName = "image.jpg",
                    ContentType = "image/jpg"
                }).Result;

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUpdateDocumentThumbnailFromPage_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateProductRequest();

                // Act
                bool ok = request.UpdateDocumentThumbnailAsync(42, 2).Result;

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenCheckForProductRight_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateProductRequest();

                // Act
                var response = request.GetRightAsync(42).Result;

                // Assert
                Assert.Equal(110, response);
            }
        }

        [Fact]
        public void WhenDownloadProductFromExtension_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateProductRequest();

                // Act
                var response = request.DownloadFileAsync(42, "pdf").Result;

                // Assert
                Assert.NotNull(response);
                using (var stream = new MemoryStream())
                {
                    response.CopyTo(stream);
                    Assert.Equal(57210, stream.Length);
                }
            }
        }

        [Fact]
        public void WhenDownloadProductFromFormatTypeId_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, ProductRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateProductRequest();

                // Act
                var response = request.DownloadFileAsync(42, 1).Result;

                // Assert
                Assert.NotNull(response);
                using (var stream = new MemoryStream())
                {
                    response.CopyTo(stream);
                    Assert.Equal(57210, stream.Length);
                }
            }
        }

        private static void ProductRequestHandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/products/42/files/1":
                case "/api/v1/products/42/files/pdf":
                    if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                    {
                        context.Response.ContentType = "application/pdf";
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        using (var file = File.OpenRead("Responses/file.pdf"))
                        {
                            file.CopyTo(context.Response.OutputStream);
                        }
                    }
                    else
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case "/api/v1/productrights/42":
                    if (context.Request.HttpMethod == "GET")
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                        {
                            context.Response.ContentType = "application/json; charset=utf-8";
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                            context.Response.OutputStream.Write("110");
                        }
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    break;
                case "/api/v1/products":
                    if (context.Request.HttpMethod == "POST")
                    {
                        requestContent = context.Request.GetRequestAsString();
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
                    requestContent = context.Request.GetRequestAsString();
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
                case "/api/v1/thumbnail/42":
                    string content = null;
                    using (var stream = new StreamReader(context.Request.InputStream))
                        content = stream.ReadToEnd();
                    if (context.Request.HttpMethod == "PUT" && content.Contains("url"))
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else if (context.Request.HttpMethod == "PUT" && content.Contains("page"))
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.OK;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else if (context.Request.HttpMethod == "PUT")
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
