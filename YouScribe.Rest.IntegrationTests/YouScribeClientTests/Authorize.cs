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
    public class Authorize
    {
        const string baseUrl = "http://localhost:8080/";

        [Fact]
        public void WhenAuthorizeUser_ThenCheckToken()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, UserAuthorizedHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                // Act
                bool isAuthorized = client.Authorize("test", "password");

                // Assert
                Assert.True(isAuthorized);
            }
        }

        [Fact]
        public void WhenAuthorizeInvalidUser_ThenCheckToken()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, UserAuthorizedHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                // Act
                bool isAuthorized = client.Authorize("test", "assword");

                // Assert
                Assert.False(isAuthorized);
            }
        }

        private static void UserAuthorizedHandler(HttpListenerContext context)
        {
            var body = new StreamReader(context.Request.InputStream).ReadToEnd();

            if (body == "UserName=test&Password=password")
                context.Response.Headers.Add(ApiUrls.AuthorizeTokenHeaderName, "OK");
        }
    }
}
