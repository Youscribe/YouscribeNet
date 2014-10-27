﻿using System;
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
            using (var client = this.CreateClient())
            {
                var response = await client.GetAsync(ApiUrls.AccountUserTypesUrl);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await this.AddErrorsAsync(response);
                    return Enumerable.Empty<Models.Accounts.UserTypeModel>();
                }
                return await this.GetObjectAsync<IEnumerable<UserTypeModel>>(response.Content);
            }
        }

        public async Task<bool> SetUserTypeAsync(Models.Accounts.UserTypeModel userType)
        {
            using (var client = this.CreateClient())
            {
                var content = this.GetContent(userType);
                var response = await client.PutAsync(ApiUrls.AccountUserTypesUrl, content);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }
    }
}
