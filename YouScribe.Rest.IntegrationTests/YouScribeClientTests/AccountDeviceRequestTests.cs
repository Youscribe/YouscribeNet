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
        const string expectedDeviceList = "<ArrayOfDeviceInformation xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/Publica.Accounts.WebServices.v1\"><DeviceInformation><DeviceId>slkgjuziegegsjgksjdgksdg5</DeviceId><DeviceTypeName>Tablet</DeviceTypeName><Id>1</Id><LastSeen>2014-10-21T14:09:03</LastSeen><Os>Windows</Os><OsVersion>8.1</OsVersion></DeviceInformation></ArrayOfDeviceInformation>";

        static string expectedDataSent = null;

        [Fact]
        public void WhenAddDevice_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.Authorize("test", "test");
                var request = client.CreateAccountDeviceRequest();

                // Act
                bool ok = request.AddDevice(DeviceTypeName.Tablet, "Windows", "8.1", "sg48e4s54gesgzeezeg");

                // Assert
                Assert.True(ok);
                Assert.Equal(expectedDataSent, "{\"DeviceTypeName\":\"Tablet\",\"Os\":\"Windows\",\"OsVersion\":\"8.1\",\"DeviceId\":\"sg48e4s54gesgzeezeg\"}");
            }
        }

        [Fact]
        public void WhenGetDevices_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.Authorize("test", "test");
                var request = client.CreateAccountDeviceRequest();

                // Act
                var devices = request.GetDevices();

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
