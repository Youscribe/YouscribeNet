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
    public class AccountDeviceRequestTests
    {
        const string expectedDeviceList = "[{\"Id\":1,\"DeviceId\":\"slkgjuziegegsjgksjdgksdg5\",\"DeviceTypeName\":\"Tablet\",\"Os\":\"Windows\",\"OsVersion\":\"8.1\",\"LastSeen\":\"2014-10-21T14:09:03\"}]";

        static string expectedDataSent = null;

        [Fact]
        public void WhenAddDevice_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "test").Wait();
                var request = client.CreateAccountDeviceRequest();

                // Act
                bool ok = request.AddDevice(DeviceTypeName.Tablet, "Windows", "8.1", "sg48e4s54gesgzeezeg").Result;

                // Assert
                Assert.True(ok);
                Assert.Equal(expectedDataSent, "{\"Os\":\"Windows\",\"OsVersion\":\"8.1\",\"DeviceTypeName\":\"Tablet\",\"DeviceId\":\"sg48e4s54gesgzeezeg\"}");
            }
        }

        [Fact]
        public void WhenGetDevices_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "test").Wait();
                var request = client.CreateAccountDeviceRequest();

                // Act
                var devices = request.GetDevices().Result;

                // Assert
                Assert.NotEmpty(devices);
                Assert.Equal("Windows", devices.First().Os);
                Assert.Equal("8.1", devices.First().OsVersion);
                Assert.Equal("Tablet", devices.First().DeviceTypeName);
                Assert.Equal("slkgjuziegegsjgksjdgksdg5", devices.First().DeviceId);
            }
        }

        private static void EventHandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/accounts/devices":
                    if (context.Request.HttpMethod == "GET")
                    {
                        context.Response.OutputStream.Write(expectedDeviceList);
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else if (context.Request.HttpMethod == "PUT")
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                        {
                            expectedDataSent = new StreamReader(context.Request.InputStream).ReadToEnd();
                            context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                        }
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
