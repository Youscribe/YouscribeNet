using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    class AccountUserTypeRequest : YouScribeRequest, IAccountUsertTypeRequest
    {
        public AccountUserTypeRequest(IRestClient client, string authorizeToken)
            : base(client, authorizeToken)
        { }

        public IEnumerable<Models.Accounts.UserTypeModel> ListAllUserTypes()
        {
            var request = this.createRequest(ApiUrls.AccountUserTypesUrl, Method.GET);

            var response = client.Execute<List<UserTypeModel>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                this.addErrors(response);
                return null;
            }
            return response.Data;
        }

        public bool SetUserType(Models.Accounts.UserTypeModel userType)
        {
            var request = this.createRequest(ApiUrls.AccountUserTypesUrl, Method.PUT);

            request.AddBody(userType);
            
            var response = client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }
    }
}
