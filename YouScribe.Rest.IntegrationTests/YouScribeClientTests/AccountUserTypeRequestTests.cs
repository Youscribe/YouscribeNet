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

        const string expectedUserTypesResponse = "<ArrayOfUserTypeModel><UserTypeModel><Id>25</Id><Label>Traducteur</Label><Name>Traducteur</Name></UserTypeModel></ArrayOfUserTypeModel>";

        [Fact]
        public void WhenListAllUserTypes_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, AccountUserTypeHandler))
            {
                var client = new YouScribeClient(baseUrl);
                var request = client.CreateAccountUserTypeRequest();

                // Act
                var userTypes = request.ListAllUserTypes();

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
            using (SimpleServer.Create(baseUrl, AccountUserTypeHandler))
            {
                var client = new YouScribeClient(baseUrl);

                client.Authorize("test", "password");

                var request = client.CreateAccountUserTypeRequest();

                // Act
                bool ok = request.SetUserType(new Models.Accounts.UserTypeModel { Id = 25 });

                // Assert
                Assert.True(ok);
            }
        }

        [Fact]
        public void WhenSetUserTypeWithUnathorizedUser_ThenCheckResponse()
        {
            // Arrange
            using (SimpleServer.Create(baseUrl, AccountUserTypeHandler))
            {
                var client = new YouScribeClient(baseUrl);

                var request = client.CreateAccountUserTypeRequest();

                // Act
                bool ok = request.SetUserType(new Models.Accounts.UserTypeModel { Id = 25 });

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
