using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;
using YouScribe.Rest.IntegrationTests.Helpers;

namespace YouScribe.Rest.IntegrationTests.YouScribeClientTests
{
    public class AccountEventRequestTests
    {
        const string expectedEventLists = "[{\"Id\":6,\"Label\":\"MemberHasSameInterest\",\"Name\":\"MemberHasSameInterest\"}]";
        static string requestContent = null;

        [Fact]
        public void WhenListAllEvents_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateAccountEventRequest();

                // Act
                var events = request.ListAllEventsAsync().Result;

                // Assert
                Assert.NotEmpty(events);
                Assert.Equal(1, events.Count());
                Assert.Equal(6, events.First().Id);
                Assert.Equal("MemberHasSameInterest", events.First().Label);
                Assert.Equal("MemberHasSameInterest", events.First().Name);
            }
        }

        [Fact]
        public void WhenSubscribeToEvent_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateAccountEventRequest();

                // Act
                bool ok = request.SubscribeToEventAsync(new Models.Accounts.AccountEventModel { Id = 6 }).Result;

                // Assert
                Assert.True(ok);
                Assert.Equal("{\"Id\":6,\"Name\":null,\"Label\":null}", requestContent);
            }
        }

        [Fact]
        public void WhenSubscribeToEventWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountEventRequest();

                // Act
                bool ok = request.SubscribeToEventAsync(new Models.Accounts.AccountEventModel { Id = 6 }).Result;

                // Assert
                Assert.False(ok);
                Assert.NotEmpty(request.Errors);
                Assert.Equal("Not connected", request.Errors.First());
            }
        }

        [Fact]
        public void WhenUnSubscribeToEvent_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateAccountEventRequest();

                // Act
                bool ok = request.UnSubscribeToEventAsync(new Models.Accounts.AccountEventModel { Id = 6 }).Result;

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUnSubscribeToEventWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountEventRequest();

                // Act
                bool ok = request.UnSubscribeToEventAsync(new Models.Accounts.AccountEventModel { Id = 6 }).Result;

                // Assert
                Assert.False(ok);
                Assert.NotEmpty(request.Errors);
                Assert.Equal("Not connected", request.Errors.First());
            }
        }

        [Fact]
        public void WhenSetEventFrequency_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateAccountEventRequest();

                // Act
                bool ok = request.SetEventFrequencyAsync(Models.Accounts.NotificationFrequency.ByWeek).Result;

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenSetEventFrequencyWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, EventHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountEventRequest();

                // Act
                bool ok = request.SetEventFrequencyAsync(Models.Accounts.NotificationFrequency.ByWeek).Result;

                // Assert
                Assert.False(ok);
                Assert.NotEmpty(request.Errors);
                Assert.Equal("Not connected", request.Errors.First());
            }
        }

        private static void EventHandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/accounts/events":
                    if (context.Request.HttpMethod == "GET")
                    {
                        context.Response.OutputStream.Write(expectedEventLists);
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else if (context.Request.HttpMethod == "PUT")
                    {
                        requestContent = context.Request.GetRequestAsString();
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    break;
                case "/api/v1/accounts/events?id=6":
                    if (context.Request.HttpMethod == "DELETE")
                    {
                        if (context.Request.Headers.AllKeys.Any(c => c == ApiUrls.AuthorizeTokenHeaderName))
                            context.Response.StatusCode = (int)HttpStatusCode.NoContent;
                        else
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    }
                    break;
                case "/api/v1/accounts/events?frequency=ByWeek":
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
