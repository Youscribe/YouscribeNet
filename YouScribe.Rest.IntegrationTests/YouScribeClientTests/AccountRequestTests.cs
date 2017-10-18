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

        static string requestContent = null;

        [Fact]
        public void WhenGetAccount_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateAccountRequest();

                // Act
                var account = request.GetCurrentAccountAsync().Result;

                // Assert
                Assert.NotNull(account);
                Assert.Equal("test", account.UserName);
            }
        }

        [Fact]
        public void WhenCreateAccount_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateAccountRequest();

                // Act
                var account = request.CreateAsync(new Models.Accounts.AccountModel { UserName = "test", Password = "password" }).Result;

                // Assert
                Assert.NotNull(account);
                Assert.Equal("test", account.UserName);
                Assert.Equal("CAAGp95NMpY8BAOtOD7F4gxpkrMnmUZCpPBWHyZAOcX723Pfez7VEQvrjZAtrDZCXPRC0wPZCxrC", account.YsAuthToken);
                Assert.Equal("{\"Id\":0,\"UserName\":\"test\",\"Password\":\"password\",\"Email\":null,\"FirstName\":null,\"LastName\":null,\"Gender\":null,\"Civility\":null,\"BirthDate\":null,\"CountryCode\":null,\"BlogUrl\":null,\"WebSiteUrl\":null,\"FacebookPage\":null,\"TwitterPage\":null,\"City\":null,\"Biography\":null,\"PhoneNumber\":null,\"EmailIsPublic\":false,\"EmailStatus\":null,\"DomainLanguageIsoCode\":null,\"TrackingId\":\"00000000-0000-0000-0000-000000000000\",\"YsAuthToken\":null}", 
                    requestContent);
            }
        }

        [Fact]
        public void WhenUpdateAccount_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.UpdateAsync(new Models.Accounts.AccountModel { Id = 42, FirstName = "kikou" }).Result;

                // Assert
                Assert.True(ok);
                Assert.Equal("{\"Id\":42,\"UserName\":null,\"Password\":null,\"Email\":null,\"FirstName\":\"kikou\",\"LastName\":null,\"Gender\":null,\"Civility\":null,\"BirthDate\":null,\"CountryCode\":null,\"BlogUrl\":null,\"WebSiteUrl\":null,\"FacebookPage\":null,\"TwitterPage\":null,\"City\":null,\"Biography\":null,\"PhoneNumber\":null,\"EmailIsPublic\":false,\"EmailStatus\":null,\"DomainLanguageIsoCode\":null,\"TrackingId\":\"00000000-0000-0000-0000-000000000000\",\"YsAuthToken\":null}", 
                    requestContent);
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
                bool ok = request.UpdateAsync(new Models.Accounts.AccountModel { Id = 42, FirstName = "kikou" }).Result;

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

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.SetSpokenLanguagesAsync(new[] { "fr", "en" }).Result;

                // Assert
                Assert.True(ok);
                Assert.Equal("Languages=fr&Languages=en", requestContent);
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
                bool ok = request.SetSpokenLanguagesAsync(new[] { "fr", "en" }).Result;

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

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.UploadPictureAsync(new Uri("http://exmple.com/image.jpg")).Result;

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
                bool ok = request.UploadPictureAsync(new Uri("http://exmple.com/image.jpg")).Result;

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

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.UploadPictureAsync(new Models.FileModel { Content = new MemoryStream(), ContentType = "image/png", FileName = "test.png" }).Result;

                // Assert
                Assert.Contains("Content-Disposition: form-data; name=file; filename=test.png; filename*=utf-8''test.png", requestContent);
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
                bool ok = request.UploadPictureAsync(new Models.FileModel { Content = new MemoryStream(), ContentType = "image/png", FileName = "test.png" }).Result;

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

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateAccountRequest();

                // Act
                bool ok = request.DeletePictureAsync().Result;

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
                bool ok = request.DeletePictureAsync().Result;

                // Assert
                Assert.False(ok);
            }
        }

        private static void AccountHandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/accounts":
                    requestContent = context.Request.GetRequestAsString();
                    if (context.Request.HttpMethod == "POST")
                    {
                        context.Response.ContentType = "application/json; charset=utf-8";
                        context.Response.StatusCode = (int)HttpStatusCode.Created;
                        context.Response.Headers.Add("YS-AUTH", "CAAGp95NMpY8BAOtOD7F4gxpkrMnmUZCpPBWHyZAOcX723Pfez7VEQvrjZAtrDZCXPRC0wPZCxrC");
                        context.Response.OutputStream.Write(expectedAccountResponse);
                    }
                    else if (context.Request.HttpMethod == "PUT")
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    else if (context.Request.HttpMethod == "GET")
                    {
                        context.Response.ContentType = "application/json; charset=utf-8";
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.OutputStream.Write(expectedAccountResponse);
                    }
                    break;
                case "/api/v1/accounts/languages":
                    requestContent = context.Request.GetRequestAsString();
                    if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                        context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                    else
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case "/api/v1/pictures?url=http%3A%2F%2Fexmple.com%2Fimage.jpg":
                case "/api/v1/pictures":
                    requestContent = context.Request.GetRequestAsString();
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
