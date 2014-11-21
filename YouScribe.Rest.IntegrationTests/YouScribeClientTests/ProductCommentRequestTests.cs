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
    public class ProductCommentRequestTests
    {
        const string baseUrl = "http://localhost:8080/";

        [Fact]
        public void WhenAskForComments_ThenCommentsAreGiven()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, ProductCommentRequesttHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateProductCommentRequest();

                int id = 5;

                // Act
                var comments = request.GetCommentsAsync(id, 0, 5, 3).Result;

                // Assert
                Assert.NotEmpty(comments.Comments);
                Assert.Equal(8, comments.Count);
                Assert.Equal(5, comments.Comments.Count());

                var first = comments.Comments.First();
                Assert.Equal("test", first.Message);
                Assert.Equal(0, first.NbReplies);
                Assert.Equal(7, first.Writer.Id);
            }
        }

        public void ProductCommentRequesttHandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/products/5/comments?skip=0&take=5&repliesTake=3":
                    context.Response.ContentType = "application/json";
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.OutputStream.Write(File.ReadAllText("Responses/ProductComments_GetCommentsResponse.txt"));
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}
