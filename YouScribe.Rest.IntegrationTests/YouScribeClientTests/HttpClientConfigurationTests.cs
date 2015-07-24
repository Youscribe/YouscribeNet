using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YouScribe.Rest.IntegrationTests.Helpers;

namespace YouScribe.Rest.IntegrationTests.YouScribeClientTests
{
    public class HttpClientConfigurationTests
    {
        [Fact]
        public void WhenSettingClientTimeout__ThenCheckValue()
        {

            YouScribeClient.DefaultTimeout = TimeSpan.FromSeconds(123);
            YouScribeClient.ClearClients();

            var client = new YouScribeClient().clientFactory();

            Assert.Equal(TimeSpan.FromSeconds(123), client.Client.BaseClient.Timeout);

        }


        [Fact]
        public void WhenSendingRequest__ThenMustTimeout()
        {
            using (SimpleServer.Create(TestHelpers.BaseUrl, 
                c => { 
                    Task.Delay(2000).Wait();
                    c.Response.Close();
                }))
            {
                YouScribeClient.DefaultTimeout = TimeSpan.FromMilliseconds(200);
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var ex = Assert.Throws<AggregateException>(() => 
                    { 
                        client.AuthorizeAsync("test", "password").Wait();

                        var request = client.CreateProductRequest();

                        // Act
                        var response = request.GetRightAsync(42).Result;

                        // Assert
                        Assert.Equal(110, response);
                    });

            }
        }

    }
}
