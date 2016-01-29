using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Accounts;
using YouScribe.Rest.Helpers;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    class AccountRequest : YouScribeRequest, IAccountRequest
    {
        public AccountRequest(Func<DisposableClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

        public Task<Models.Accounts.AccountGetModel> GetCurrentAccountAsync()
        {
            return this.GetAsync<Models.Accounts.AccountGetModel>(ApiUrls.AccountUrl);
        }

        public async Task<Models.Accounts.AccountModel> CreateAsync(Models.Accounts.AccountModel account, int? validityInHours = null)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var content = this.GetContent(account);
                var url = ApiUrls.AccountUrl;
                if (validityInHours.HasValue)
                    url = ApiUrls.AccountUrl + "?validityInHours=" + validityInHours;
                var response = await client.PostAsync(this.GetUri(url), content).ConfigureAwait(false);

                if (response.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    await this.AddErrorsAsync(response).ConfigureAwait(false);
                    return null;
                }  

                var model = await this.GetObjectAsync<AccountModel>(response.Content).ConfigureAwait(false);
                var tokenValue = response.Headers.GetValues("YS-AUTH");
                model.YsAuthToken = tokenValue.FirstOrDefault();
                return model;
            }
        }

        public async Task<bool> UpdateAsync(Models.Accounts.AccountModel account)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var content = this.GetContent(account);
                var response = await client.PutAsync(this.GetUri(ApiUrls.AccountUrl), content).ConfigureAwait(false);

                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<bool> SetSpokenLanguagesAsync(IEnumerable<string> languages)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var content = new FormUrlEncodedContent(
                    languages.Select(c => new KeyValuePair<string, string>("Languages", c))
                );
                var response = await client.PutAsync(this.GetUri(ApiUrls.AccountLanguagesUrl), content).ConfigureAwait(false);

                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<bool> UploadPictureAsync(Uri uri)
        {
            if (uri.IsValid() == false)
                throw new ArgumentException("Uri is not valid", "uri");
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.PictureUrl;
                var dico = new Dictionary<string, string>(){
                {"url", uri.ToString()}
            };
                url = url + "?" + dico.ToQueryString();
                var response = await client.PostAsync(this.GetUri(url), null).ConfigureAwait(false);


                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<bool> UploadPictureAsync(Models.FileModel image)
        {
            if (image.IsValid == false)
                throw new ArgumentException("image is not valid", "image");

            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(image.Content), "file", image.FileName);
                var response = await client.PostAsync(this.GetUri(ApiUrls.PictureUrl), content).ConfigureAwait(false);

                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<bool> DeletePictureAsync()
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var response = await client.DeleteAsync(this.GetUri(ApiUrls.PictureUrl)).ConfigureAwait(false);

                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }
    }
}
