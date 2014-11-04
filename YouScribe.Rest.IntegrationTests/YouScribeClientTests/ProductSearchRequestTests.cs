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
                        quicksearch = "()pouet$&"
                    }).Result;

                // Assert
                Assert.Empty(request.Errors);
                Assert.NotNull(results);
                Assert.Equal(1, results.TotalResults);
                Assert.NotEmpty(results.Products);
                Assert.Equal("bouh", results.Products.First().Title);
            }
        }

        private static void EventHandler(HttpListenerContext context)
        {
            Console.WriteLine(context.Request.RawUrl);
            switch (context.Request.RawUrl)
            {
                case "/api/v1/products/search?id=5,9,18&quicksearch=%28%29pouet%24%26&skip=0&take=10":
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.OutputStream.Write(File.ReadAllText("Responses/ProductSearch_Search.txt"));
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}
