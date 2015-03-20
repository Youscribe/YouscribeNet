using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;
using Youscribe.Rest.Cryptography;
using YouScribe.Rest.IntegrationTests.Helpers;

namespace YouScribe.Rest.IntegrationTests.YouScribeClientTests
{
    public class YousScribeHMACHttpClientTest
    {
        static string requestUrl;

        [Fact]
        public void WhenRequest_ThenHeadersOk()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var encoding = new System.Text.UnicodeEncoding();
                byte[] secretKey = encoding.GetBytes("DummyKey");
                int applicationId = 1;
                IHMAC hmac = new TestHMAC();
                var client = new YouScribeClient(TestHelpers.BaseUrl, YousScribeHMACHttpClientDecorator.GetBaseClientFactory(secretKey, applicationId, hmac));
                YousScribeHMACHttpClientDecorator httpClient = (YousScribeHMACHttpClientDecorator)client.clientFactory();

                // Act
                httpClient.GetAsync(TestHelpers.BaseUrl);

                // Assert
                Assert.NotNull(httpClient.BaseClient.DefaultRequestHeaders.Authorization);
                Assert.NotNull(httpClient.BaseClient.DefaultRequestHeaders.Date);
                Assert.NotNull(httpClient.BaseClient.DefaultRequestHeaders.GetValues(ApiUrls.HMACAuthenticateRandomKeyHeader));
            }
        }

        private static void EventHandler(HttpListenerContext context)
        {
            requestUrl = context.Request.RawUrl;
            switch (context.Request.RawUrl)
            {
                case "/api/v2/products/1470/suggests?domainLanguage=fr&take=3":
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
