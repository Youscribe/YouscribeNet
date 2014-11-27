using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;
using YouScribe.Rest.IntegrationTests.Helpers;

namespace YouScribe.Rest.IntegrationTests.YouScribeClientTests
{
    public class AccountPublisherRequestTests
    {
        const string baseUrl = "http://localhost:8080/";
        static string requestContent = null;

        [Fact]
        public void WhenSetAsPaypalPublisher_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, Publisherandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateAccountPublisherRequest();

                // Act
                bool ok = request.SetAsPaypalPublisherAsync(new Models.Accounts.AccountPublisherPaypalModel()).Result;

                // Assert
                Assert.True(ok);
                Assert.Equal("{\"PaypalEmail\":null,\"IsProfessional\":false,\"CorporateName\":null,\"SiretNumber\":null,\"VATNumber\":null,\"Street\":null,\"Street2\":null,\"ZipCode\":null,\"State\":null,\"City\":null,\"CountryCode\":null,\"Civility\":0,\"FirstName\":null,\"LastName\":null,\"PhoneNumber\":null}", requestContent);
            }
        }

        [Fact]
        public void WhenSetAsPaypalPublisherWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, Publisherandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountPublisherRequest();

                // Act
                bool ok = request.SetAsPaypalPublisherAsync(new Models.Accounts.AccountPublisherPaypalModel()).Result;

                // Assert
                Assert.False(ok);
                Assert.NotEmpty(request.Error.Messages);
                Assert.Equal("Not connected", request.Error.Messages.First());
            }
        }

        [Fact]
        public void WhenSetAsTransferPublisher_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, Publisherandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateAccountPublisherRequest();

                // Act
                bool ok = request.SetAsTransferPublisherAsync(new Models.Accounts.AccountPublisherTransferModel()).Result;

                // Assert
                Assert.True(ok);
                Assert.Equal("{\"BankName\":null,\"IBAN\":null,\"BIC\":null,\"IsProfessional\":false,\"CorporateName\":null,\"SiretNumber\":null,\"VATNumber\":null,\"Street\":null,\"Street2\":null,\"ZipCode\":null,\"State\":null,\"City\":null,\"CountryCode\":null,\"Civility\":0,\"FirstName\":null,\"LastName\":null,\"PhoneNumber\":null}", 
                    requestContent);
            }
        }

        [Fact]
        public void WhenSetAsTransferPublisherWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, Publisherandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountPublisherRequest();

                // Act
                bool ok = request.SetAsTransferPublisherAsync(new Models.Accounts.AccountPublisherTransferModel()).Result;

                // Assert
                Assert.False(ok);
                Assert.NotEmpty(request.Error.Messages);
                Assert.Equal("Not connected", request.Error.Messages.First());
            }
        }

        private static void Publisherandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/accounts/paypalpublisher":
                    if (context.Request.HttpMethod == "PUT")
                    {
                        requestContent = context.Request.GetRequestAsString();
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    break;
                case "/api/v1/accounts/transferpublisher":
                    if (context.Request.HttpMethod == "PUT")
                    {
                        requestContent = context.Request.GetRequestAsString();
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
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
