using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;
using YouScribe.Rest.IntegrationTests.Helpers;

namespace YouScribe.Rest.IntegrationTests.YouScribeClientTests
{
    public class EmbedRequestTests
    {
        const string baseUrl = "http://localhost:8080/";

        const string expectedEmbed1 = "{\"Content\":\"embed1\"}";
        const string expectedEmbed2 = "{\"Content\":\"embed2\"}";
        const string expectedEmbed3 = "{\"Content\":\"embed3\"}";
        const string expectedEmbed4 = "{\"Content\":\"embed4\"}";

        [Fact]
        public void WhenGenerateEmbedIframeTag_ThenCheckCode()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, GenerateEmbledHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateEmbedRequest();

                int productId = 1;

                // Act
                var tag = request.GenerateIframeTagAsync(productId).Result;

                // Assert
                Assert.NotEmpty(tag);
                Assert.Equal("embed1", tag);
            }
        }

        [Fact]
        public void WhenGenerateEmbedIframeTagWithFeatures_ThenCheckCode()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, GenerateEmbledHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateEmbedRequest();

                int productId = 1;

                // Act
                var tag = request.GenerateIframeTagAsync(productId, new Models.Products.EmbedGenerateModel { Width = 600, Height = 300 }).Result;

                // Assert
                Assert.NotEmpty(tag);
                Assert.Equal("embed2", tag);
            }
        }

        [Fact]
        public void WhenGeneratePrivateEmbedIframeTag_ThenCheckCode()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, GenerateEmbledHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateEmbedRequest();

                int productId = 1;

                // Act
                var tag = request.GeneratePrivateIframeTagAsync(productId).Result;

                // Assert
                Assert.NotEmpty(tag);
                Assert.Equal("embed3", tag);
            }
        }

        [Fact]
        public void WhenGeneratePrivateEmbedIframeTagWithFeatures_ThenCheckCode()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, GenerateEmbledHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateEmbedRequest();

                int productId = 1;

                // Act
                var tag = request.GeneratePrivateIframeTagAsync(productId, new Models.Products.PrivateEmbedGenerateModel { Width = 600, Height = 300, AccessPeriod = "1d" }).Result;

                // Assert
                Assert.NotEmpty(tag);
                Assert.Equal("embed4", tag);
            }
        }


        [Fact]
        public void WhenGeneratePrivateEmbedIframeTagWithUnauthorizedUser_ThenCheckCode()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, GenerateEmbledHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateEmbedRequest();

                int productId = 1;

                // Act
                var tag = request.GeneratePrivateIframeTagAsync(productId).Result;

                // Assert
                Assert.Empty(tag);
                Assert.NotEmpty(request.Error.Messages);
                Assert.Contains("Not connected", request.Error.Messages.First());
            }
        }


        private static void GenerateEmbledHandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/embed/1":
                    context.Response.OutputStream.Write(expectedEmbed1);
                    break;
                case "/api/v1/embed/1?height=300&width=600":
                    context.Response.OutputStream.Write(expectedEmbed2);
                    break;
                case "/api/v1/embed/private?id=1":
                    if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                        context.Response.OutputStream.Write(expectedEmbed3);
                    else
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case "/api/v1/embed/private?id=1&height=300&width=600&accessPeriod=1d":
                    if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                        context.Response.OutputStream.Write(expectedEmbed4);
                    else
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
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
