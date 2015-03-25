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
        public AccountUserTypeRequest(Func<IYousScribeHttpClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

        public async Task<IEnumerable<Models.Accounts.UserTypeModel>> ListAllUserTypesAsync()
        {
            var client = this.CreateClient();
            var response = await client.GetAsync(this.GetUri(ApiUrls.AccountUserTypesUrl)).ConfigureAwait(false);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await this.AddErrorsAsync(response).ConfigureAwait(false);
                return Enumerable.Empty<Models.Accounts.UserTypeModel>();
            }
            return await this.GetObjectAsync<IEnumerable<UserTypeModel>>(response.Content).ConfigureAwait(false);
        }

        public async Task<bool> SetUserTypeAsync(Models.Accounts.UserTypeModel userType)
        {
            var client = this.CreateClient();
            var content = this.GetContent(userType);
            var response = await client.PutAsync(this.GetUri(ApiUrls.AccountUserTypesUrl), content).ConfigureAwait(false);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
        }
    }
}
