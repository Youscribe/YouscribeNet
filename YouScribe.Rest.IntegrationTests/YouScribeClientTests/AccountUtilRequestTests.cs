using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YouScribe.Rest.IntegrationTests.Helpers;

namespace YouScribe.Rest.IntegrationTests.YouScribeClientTests
{
    public class AccountUtilRequestTests
    {
        [Fact]
        public void WhenGeneratePassword_ThenOk()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountUtilRequest();

                // Act
                var password = request.GeneratePasswordAsync(8, 10).Result;

                // Assert
                Assert.Empty(request.Error.Messages);
                Assert.Equal("toto", password);
            }
        }

        [Fact]
        public void WhenGenerateUserNameFromEmail_ThenOk()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountUtilRequest();

                // Act
                var userName = request.GetUserNameFromEmailAsync("me.show@gmail.com").Result;

                // Assert
                Assert.Empty(request.Error.Messages);
                Assert.Equal("me.show", userName);
            }
        }

        private static void EventHandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/accounts/generated-passwords?minLength=8&maxLength=10":
                    if (context.Request.HttpMethod == "POST")
                    {
                        context.Response.ContentType = "application/json; charset=utf-8";
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.OutputStream.Write("\"toto\"");
                    }
                    break;
                case "/api/v1/accounts/unique-usernames?email=me.show%40gmail.com":
                    if (context.Request.HttpMethod == "POST")
                    {
                        context.Response.ContentType = "application/json; charset=utf-8";
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.OutputStream.Write("\"me.show\"");
                    }
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}
