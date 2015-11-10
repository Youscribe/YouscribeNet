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
        public AccountUtilRequest(Func<DisposableClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        {
        }

        public async Task<string> GeneratePasswordAsync(int minLength, int maxLength)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
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
        }

        public async Task<string> GetUserNameFromEmailAsync(string email)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
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
        }

        public async Task<bool> ForgotPasswordAsync(string userNameOrEmail)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
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

        public async Task<bool> ChangeEmailAsync(Models.Accounts.AccountModel account)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var content = this.GetContent(account);
                var response = await client.PutAsync(this.GetUri("api/v1/accounts/change-email"), content).ConfigureAwait(false);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
            }
        }

        public async Task<bool> DeleteAccountAsync()
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var response = await client.DeleteAsync(this.GetUri("api/v1/accounts/delete-account")).ConfigureAwait(false);
                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
            }
        }
    }
}
