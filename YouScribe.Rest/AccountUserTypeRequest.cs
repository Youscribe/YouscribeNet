using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    class AccountUserTypeRequest : YouScribeRequest, IAccountUsertTypeRequest
    {
        public AccountUserTypeRequest(Func<HttpClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

        public async Task<IEnumerable<Models.Accounts.UserTypeModel>> ListAllUserTypesAsync()
        {
            var request = this.createRequest(ApiUrls.AccountUserTypesUrl, Method.GET);

            var response = client.Execute<List<UserTypeModel>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await this.AddErrorsAsync(response);
                return Enumerable.Empty<Models.Accounts.UserTypeModel>();
            }
            return response.Data;
        }

        public async Task<bool> SetUserTypeAsync(Models.Accounts.UserTypeModel userType)
        {
            var request = this.createRequest(ApiUrls.AccountUserTypesUrl, Method.PUT);

            request.AddBody(userType);
            
            var response = client.Execute(request);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }
    }
}
