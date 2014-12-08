using System;
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
            var client = this.CreateClient();
            var url = "api/v1/accounts/generated-passwords";
            var dico = new Dictionary<string, string>()
            {
                { "minLength", minLength.ToString() },
                { "maxLength", maxLength.ToString() },
            };
            url = url + "?" + dico.ToQueryString();
            var response = await client.PostAsync(this.GetUri(url), null).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                await this.AddErrorsAsync(response).ConfigureAwait(false);
            return await this.GetObjectAsync<string>(response.Content).ConfigureAwait(false);
        }

        public async Task<string> GetUserNameFromEmailAsync(string email)
        {
            var client = this.CreateClient();
            var url = "api/v1/accounts/unique-usernames";
            url = url + "?email=" + System.Uri.EscapeDataString(email);
            var response = await client.PostAsync(this.GetUri(url), null).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                await this.AddErrorsAsync(response).ConfigureAwait(false);
                return null;
            }
            return await this.GetObjectAsync<string>(response.Content).ConfigureAwait(false);
        }

        public async Task<bool> ForgotPasswordAsync(string userNameOrEmail)
        {
            var client = this.CreateClient();
            var url = "api/v1/accounts/forgot-passwords";
            url = url + "?userNameOrEmail=" + System.Uri.EscapeDataString(userNameOrEmail);
            var response = await client.PutAsync(this.GetUri(url), null).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                await this.AddErrorsAsync(response).ConfigureAwait(false);
                return false;
            }
            return true;
        }


    }
}
