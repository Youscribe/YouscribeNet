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
    public class ProductSuggestRequestTests
    {
        static string requestUrl;

        [Fact]
        public void WhenSearching_ThenOk()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateProductSuggestRequest();

                // Act
                var results = request.GetSuggestAsync(1470).Result;

                // Assert
                Assert.Empty(request.Error.Messages);
                Assert.NotNull(results);
                Assert.NotEmpty(results);
                Assert.Equal("bouh", results.First().Title);
            }
        }

        private static void EventHandler(HttpListenerContext context)
        {
            requestUrl = context.Request.RawUrl;
            switch (context.Request.RawUrl)
            {
                case "/api/v1/products/1470/suggests?domainLanguage=fr&take=3":
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Write(File.ReadAllText("Responses/ProductSuggest_Get.txt"));
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}
