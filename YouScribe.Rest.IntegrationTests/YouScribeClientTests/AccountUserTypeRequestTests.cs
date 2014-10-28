using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;
using YouScribe.Rest.IntegrationTests.Helpers;

namespace YouScribe.Rest.IntegrationTests.YouScribeClientTests
{
    public class AccountUserTypeRequestTests
    {
        const string baseUrl = "http://localhost:8080/";

        const string expectedUserTypesResponse = "[{\"Id\":25,\"Label\":\"Traducteur\",\"Name\":\"Traducteur\"}]";

        static string requestContent = null;

        [Fact]
        public void WhenListAllUserTypes_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountUserTypeHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);
                var request = client.CreateAccountUserTypeRequest();

                // Act
                var userTypes = request.ListAllUserTypesAsync().Result;

                // Assert
                Assert.NotNull(userTypes);
                Assert.NotEmpty(userTypes);
                Assert.Equal(25, userTypes.First().Id);
                Assert.Equal("Traducteur", userTypes.First().Label);
                Assert.Equal("Traducteur", userTypes.First().Name);
            }
        }

        [Fact]
        public void WhenSetUserType_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountUserTypeHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                client.AuthorizeAsync("test", "password").Wait();

                var request = client.CreateAccountUserTypeRequest();

                // Act
                bool ok = request.SetUserTypeAsync(new Models.Accounts.UserTypeModel { Id = 25 }).Result;

                // Assert
                Assert.True(ok);
                Assert.Equal("{\"Id\":25,\"Name\":null,\"Label\":null}", requestContent);
            }
        }

        [Fact]
        public void WhenSetUserTypeWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(TestHelpers.BaseUrl, AccountUserTypeHandler))
            {
                var client = new YouScribeClient(TestHelpers.BaseUrl);

                var request = client.CreateAccountUserTypeRequest();

                // Act
                bool ok = request.SetUserTypeAsync(new Models.Accounts.UserTypeModel { Id = 25 }).Result;

                // Assert
                Assert.False(ok);
            }
        }

        private static void AccountUserTypeHandler(HttpListenerContext context)
        {
            switch (context.Request.RawUrl)
            {
                case "/api/v1/accounts/usertypes":
                    if (context.Request.HttpMethod == "GET")
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        context.Response.OutputStream.Write(expectedUserTypesResponse);
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
