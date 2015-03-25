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
    public class LibraryRequestTests
    {
        const string baseUrl = "http://localhost:8080/";

        [Fact]
        public void WhenGetLibrary_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, LibraryRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateLibraryRequest();

                int id = 1;

                // Act
                var library = request.GetAsync(id).Result;

                // Assert
                Assert.NotNull(library);
                Assert.Equal("MyDownloads", library.TypeName);
            }
        }

        [Fact]
        public void WhenGetLibraryByTypeName_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, LibraryRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateLibraryRequest();

                // Act
                var library = request.GetAsync("MyDownloads").Result;

                // Assert
                Assert.NotNull(library);
                Assert.Equal("MyDownloads", library.TypeName);
            }
        }

        [Fact]
        public void WhenGetLibraries_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, LibraryRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateLibraryRequest();

                // Act
                var libraries = request.GetAsync().Result;

                // Assert
                Assert.NotEmpty(libraries);
                Assert.Equal(3, libraries.Count());
            }
        }

        [Fact]
        public void WhenAddProductWithLibId_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, LibraryRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateLibraryRequest();

                // Act
                var ok = request.AddProductAsync(1, 10, true).Result;

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenAddProductWithLibTypeName_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, LibraryRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateLibraryRequest();

                // Act
                var ok = request.AddProductAsync("MyDownloads", 10, false).Result;

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenDeleteProductById_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, LibraryRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateLibraryRequest();

                // Act
                var ok = request.DeleteProductAsync(1, 10).Result;
                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenDeleteProductByTypeName_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, LibraryRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateLibraryRequest();

                // Act
                var ok = request.DeleteProductAsync("MyDownloads", 10).Result;
                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenGetByProductId_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, LibraryRequestHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateLibraryRequest();

                // Act
                var data = request.GetByProductIdAsync(1).Result;

                // Assert
                Assert.NotEmpty(data);
                Assert.Equal(123, data.FirstOrDefault());
                Assert.Equal(456, data.LastOrDefault());
            }
        }

        public void LibraryRequestHandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/libraries":
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.OutputStream.Write(File.ReadAllText("Responses/Libraries_GetAllResponse.txt"));
                    break;
                case "/api/v1/libraries/MyDownloads":
                case "/api/v1/libraries/1":
                    if (context.Request.HttpMethod == "GET")
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.ContentEncoding = Encoding.UTF8;
                        var test = context.Request.ContentEncoding;
                        context.Response.OutputStream.Write(File.ReadAllText("Responses/Libraries_GetResponse.txt"));
                    }
                    else if (context.Request.HttpMethod == "DELETE")
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                    }
                    break;
                case "/api/v1/libraries/1/product/10":
                case "/api/v1/libraries/MyDownloads/product/10":
                    if (context.Request.HttpMethod == "PUT")
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                    }
                    else if (context.Request.HttpMethod == "DELETE")
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                    }
                    break;
                case "/api/v1/libraries/product/1":
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.OutputStream.Write("[123, 456]");
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}
