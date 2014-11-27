﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    class AccountUtilRequest : YouScribeRequest, IAccountUtilRequest
    {
        public AccountUtilRequest(Func<HttpClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        {
        }

        public async Task<string> GeneratePasswordAsync(int minLength, int maxLength)
        {
            using (var client = this.CreateClient())
            {
                var url = "api/v1/accounts/generated-passwords";
                var dico = new Dictionary<string, string>()
                {
                    { "minLength", minLength.ToString() },
                    { "maxLength", maxLength.ToString() },
                };
                url = url + "?" + dico.ToQueryString();
                var response = await client.PostAsync(url, null);

                if (!response.IsSuccessStatusCode)
                    await this.AddErrorsAsync(response);
                return await this.GetObjectAsync<string>(response.Content);
            }
        }

        public async Task<string> GetUserNameFromEmailAsync(string email)
        {
            using (var client = this.CreateClient())
            {
                var url = "api/v1/accounts/unique-usernames";
                url = url + "?email=" + System.Uri.EscapeDataString(email);
                var response = await client.PostAsync(url, null);

                if (!response.IsSuccessStatusCode)
                {
                    await this.AddErrorsAsync(response);
                    return null;
                }
                return await this.GetObjectAsync<string>(response.Content);
            }
        }
    }
}
