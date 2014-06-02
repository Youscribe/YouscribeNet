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

                int id = 53;

                // Act
                var library = request.Get(id);

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
                var libraries = request.Get();

                // Assert
                Assert.NotEmpty(libraries);
                Assert.Equal(3, libraries.Count());
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
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = Encoding.UTF8;
                    var test = context.Request.ContentEncoding;
                    context.Response.OutputStream.Write(File.ReadAllText("Responses/Libraries_GetResponse.txt"));
                    break;
                case "/api/v1/libraries/1/products/2":
                    if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                        context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                    else
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}
