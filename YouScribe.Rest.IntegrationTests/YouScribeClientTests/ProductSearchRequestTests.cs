using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;
using YouScribe.Rest.IntegrationTests.Helpers;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest.IntegrationTests.YouScribeClientTests
{
    public class ProductSearchRequestTests
    {
        static string requestUrl;
        static string postData;

        [Fact]
        public void WhenSearching_ThenOk()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateProductSearchRequest();

                // Act
                var results = request.SearchProductsAsync(
                    new ProductSearchInputModel()
                    {
                        id = new List<int>() { 5, 9, 18 },
                        domain_language = "fr",
                        quicksearch = "pouet$&",
                        sensibility_id = new List<int>() { 1, 2, 3 }
                    }).Result;

                // Assert
                Assert.Empty(request.Error.Messages);
                Assert.NotNull(results);
                Assert.Equal(1, results.TotalResults);
                Assert.NotEmpty(results.Products);
                Assert.Equal("bouh", results.Products.First().Title);
                Assert.Equal(2, results.Products.First().ExtractPublicFormatExtensions.Count());
                Assert.Equal(2, results.Products.First().PublicFormatExtensions.Count());
                var theme = results.Products.First().Theme;
                Assert.NotNull(theme);
                Assert.Equal(138, theme.Id);
            }
        }
        

        private static void EventHandler(HttpListenerContext context)
        {
            requestUrl = context.Request.RawUrl;
            switch (context.Request.RawUrl)
            {
                case "/api/v1/products/search":
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    postData = context.Request.GetRequestAsString();
                    context.Response.OutputStream.Write(File.ReadAllText("Responses/ProductSearch_Search.txt"));
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}
