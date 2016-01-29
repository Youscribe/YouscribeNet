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
        public AccountUserTypeRequest(Func<DisposableClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

        public Task<IEnumerable<Models.Accounts.UserTypeModel>> ListAllUserTypesAsync()
        {
            return this.GetEnumerableAsync<Models.Accounts.UserTypeModel>(ApiUrls.AccountUserTypesUrl);
        }

        public async Task<bool> SetUserTypeAsync(Models.Accounts.UserTypeModel userType)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var content = this.GetContent(userType);
                var response = await client.PutAsync(this.GetUri(ApiUrls.AccountUserTypesUrl), content).ConfigureAwait(false);

                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }
    }
}
