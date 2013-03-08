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

        const string expectedEmbed1 = "<EmbedModel><Content>embed1</Content></EmbedModel>";
        const string expectedEmbed2 = "<EmbedModel><Content>embed2</Content></EmbedModel>";
        const string expectedEmbed3 = "<EmbedModel><Content>embed3</Content></EmbedModel>";
        const string expectedEmbed4 = "<EmbedModel><Content>embed4</Content></EmbedModel>";

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
                var tag = request.GenerateIframeTag(productId);

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
                var tag = request.GenerateIframeTag(productId, new Models.Products.EmbedGenerateModel { Width = 600, Height = 300 });

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
                client.Authorize("test", "password");

                var request = client.CreateEmbedRequest();

                int productId = 1;

                // Act
                var tag = request.GeneratePrivateIframeTag(productId);

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
                client.Authorize("test", "password");

                var request = client.CreateEmbedRequest();

                int productId = 1;

                // Act
                var tag = request.GeneratePrivateIframeTag(productId, new Models.Products.PrivateEmbedGenerateModel { Width = 600, Height = 300, AccessPeriod = "1d" });

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
                var tag = request.GeneratePrivateIframeTag(productId);

                // Assert
                Assert.Empty(tag);
                Assert.NotEmpty(request.Errors);
                Assert.Equal("Not connected", request.Errors.First());
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
