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

        [Fact]
        public void WhenSetAsPaypalPublisher_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, Publisherandler))
            {
                var client = new YouScribeClient(baseUrl);
                client.Authorize("test", "password");

                var request = client.CreateAccountPublisherRequest();

                // Act
                bool ok = request.SetAsPaypalPublisher(new Models.Accounts.AccountPublisherPaypalModel());

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenSetAsPaypalPublisherWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, Publisherandler))
            {
                var client = new YouScribeClient(baseUrl);

                var request = client.CreateAccountPublisherRequest();

                // Act
                bool ok = request.SetAsPaypalPublisher(new Models.Accounts.AccountPublisherPaypalModel());

                // Assert
                Assert.False(ok);
                Assert.NotEmpty(request.Errors);
                Assert.Equal("Not connected", request.Errors.First());
            }
        }

        [Fact]
        public void WhenSetAsTransferPublisher_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, Publisherandler))
            {
                var client = new YouScribeClient(baseUrl);
                client.Authorize("test", "password");

                var request = client.CreateAccountPublisherRequest();

                // Act
                bool ok = request.SetAsTransferPublisher(new Models.Accounts.AccountPublisherTransferModel());

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenSetAsTransferPublisherWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, Publisherandler))
            {
                var client = new YouScribeClient(baseUrl);

                var request = client.CreateAccountPublisherRequest();

                // Act
                bool ok = request.SetAsTransferPublisher(new Models.Accounts.AccountPublisherTransferModel());

                // Assert
                Assert.False(ok);
                Assert.NotEmpty(request.Errors);
                Assert.Equal("Not connected", request.Errors.First());
            }
        }

        private static void Publisherandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/accounts/paypalpublisher":
                    if (context.Request.HttpMethod == "PUT")
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    break;
                case "/api/v1/accounts/transferpublisher":
                    if (context.Request.HttpMethod == "PUT")
                    {
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
