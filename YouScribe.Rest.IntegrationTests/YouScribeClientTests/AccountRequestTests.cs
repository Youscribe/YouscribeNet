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
    public class AccountRequestTests
    {
        const string baseUrl = "http://localhost:8080/";

        const string expectedAccountResponse = "{\"Id\":0,\"UserName\":\"test\"}";

        [Fact]
        public void WhenCreateAccount_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateAccountRequest();

                // Act
                var account = request.Create(new Models.Accounts.AccountModel { UserName = "test", Password = "password" });

                // Assert
                Assert.NotNull(account);
                Assert.Equal("test", account.UserName);
            }
        }

        [Fact]
        public void WhenUpdateAccount_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.Authorize("test", "password");

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.Update(new Models.Accounts.AccountModel { Id = 42, FirstName = "kikou" });

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUpdateAccountWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.Update(new Models.Accounts.AccountModel { Id = 42, FirstName = "kikou" });

                // Assert
                Assert.False(ok);
            }
        }

        [Fact]
        public void WhenSetSpokenLanguages_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.Authorize("test", "password");

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.SetSpokenLanguages(new[] { "fr", "en" });

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenSetSpokenLanguagesWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.SetSpokenLanguages(new[] { "fr", "en" });

                // Assert
                Assert.False(ok);
            }
        }

        [Fact]
        public void WhenUploadPictureFromUrl_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.Authorize("test", "password");

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.UploadPicture(new Uri("http://exmple.com/image.jpg"));

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUploadPictureFromUrlWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.UploadPicture(new Uri("http://exmple.com/image.jpg"));

                // Assert
                Assert.False(ok);
            }
        }


        [Fact]
        public void WhenUploadPictureFromLocalFile_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.Authorize("test", "password");

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.UploadPicture(new Models.FileModel { Content = new MemoryStream(), ContentType = "image/png", FileName = "test.png" });

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUploadPictureFromLocalFileWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.UploadPicture(new Models.FileModel { Content = new MemoryStream(), ContentType = "image/png", FileName = "test.png" });

                // Assert
                Assert.False(ok);
            }
        }

        [Fact]
        public void WhenDeletePicture_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.Authorize("test", "password");

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.DeletePicture();

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenDeletePictureWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.DeletePicture();

                // Assert
                Assert.False(ok);
            }
        }

        private static void AccountHandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/accounts":
                    if (context.Request.HttpMethod == "POST")
                    {
                        context.Response.ContentType = "application/json; charset=utf-8";
                        context.Response.StatusCode = (int)HttpStatusCode.Created;
                        context.Response.OutputStream.Write(expectedAccountResponse);
                    }
                    else if (context.Request.HttpMethod == "PUT")
                    {
                        if (context.Request.HttpMethod == "PUT")
                        {
                            if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                                context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                            else
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        }
                    }
                    break;
                case "/api/v1/accounts/languages":
                    if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                        context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                    else
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case "/api/v1/pictures?url=http%3A%2F%2Fexmple.com%2Fimage.jpg":
                case "/api/v1/pictures":
                    if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                        context.Response.StatusCode = context.Request.HttpMethod == "DELETE" ? (int)HttpStatusCode.NoContent : (int)HttpStatusCode.OK;
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
