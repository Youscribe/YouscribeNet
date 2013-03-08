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
        const string baseUrl = "http://localhost:8080/";

        const string expectedEventLists = "<ArrayOfAccountEventModel><AccountEventModel><Id>6</Id><Label>MemberHasSameInterest</Label><Name>MemberHasSameInterest</Name></AccountEventModel></ArrayOfAccountEventModel>";

        [Fact]
        public void WhenListAllEvents_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, EventHandler))
            {
                var client = new YouScribeClient(baseUrl);
                var request = client.CreateAccountEventRequest();

                // Act
                var events = request.ListAllEvents();

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
            using (SimpleServer.Create(baseUrl, EventHandler))
            {
                var client = new YouScribeClient(baseUrl);
                client.Authorize("test", "password");

                var request = client.CreateAccountEventRequest();

                // Act
                bool ok = request.SubscribeToEvent(new Models.Accounts.AccountEventModel { Id = 6 });

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenSubscribeToEventWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, EventHandler))
            {
                var client = new YouScribeClient(baseUrl);

                var request = client.CreateAccountEventRequest();

                // Act
                bool ok = request.SubscribeToEvent(new Models.Accounts.AccountEventModel { Id = 6 });

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
            using (SimpleServer.Create(baseUrl, EventHandler))
            {
                var client = new YouScribeClient(baseUrl);
                client.Authorize("test", "password");

                var request = client.CreateAccountEventRequest();

                // Act
                bool ok = request.UnSubscribeToEvent(new Models.Accounts.AccountEventModel { Id = 6 });

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenUnSubscribeToEventWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, EventHandler))
            {
                var client = new YouScribeClient(baseUrl);

                var request = client.CreateAccountEventRequest();

                // Act
                bool ok = request.UnSubscribeToEvent(new Models.Accounts.AccountEventModel { Id = 6 });

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
            using (SimpleServer.Create(baseUrl, EventHandler))
            {
                var client = new YouScribeClient(baseUrl);
                client.Authorize("test", "password");

                var request = client.CreateAccountEventRequest();

                // Act
                bool ok = request.SetEventFrequency(Models.Accounts.NotificationFrequency.ByWeek);

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenSetEventFrequencyWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, EventHandler))
            {
                var client = new YouScribeClient(baseUrl);

                var request = client.CreateAccountEventRequest();

                // Act
                bool ok = request.SetEventFrequency(Models.Accounts.NotificationFrequency.ByWeek);

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
