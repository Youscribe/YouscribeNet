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
        public AccountRequest(Func<HttpClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

        public async Task<Models.Accounts.AccountGetModel> GetCurrentAccountAsync()
        {
            var client = this.CreateClient();
            var response = await client.GetAsync(this.GetUri(ApiUrls.AccountUrl));

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await this.AddErrorsAsync(response);
                return null;
            }
            return await this.GetObjectAsync<AccountGetModel>(response.Content);
        }

        public async Task<Models.Accounts.AccountModel> CreateAsync(Models.Accounts.AccountModel account)
        {
            var client = this.CreateClient();
            var content = this.GetContent(account);
            var response = await client.PostAsync(this.GetUri(ApiUrls.AccountUrl), content);

            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                await this.AddErrorsAsync(response);
                return null;
            }
            return await this.GetObjectAsync<AccountModel>(response.Content);
        }

        public async Task<bool> UpdateAsync(Models.Accounts.AccountModel account)
        {
            var client = this.CreateClient();
            var content = this.GetContent(account);
            var response = await client.PutAsync(this.GetUri(ApiUrls.AccountUrl), content);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<bool> SetSpokenLanguagesAsync(IEnumerable<string> languages)
        {
            var client = this.CreateClient();
            var content = new FormUrlEncodedContent(
                languages.Select(c => new KeyValuePair<string, string>("Languages", c))
            );
            var response = await client.PutAsync(this.GetUri(ApiUrls.AccountLanguagesUrl), content);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<bool> UploadPictureAsync(Uri uri)
        {
            if (uri.IsValid() == false)
                throw new ArgumentException("Uri is not valid", "uri");
            var client = this.CreateClient();
            var url = ApiUrls.PictureUrl;
            var dico = new Dictionary<string, string>(){
                {"url", uri.ToString()}
            };
            url = url + "?" + dico.ToQueryString();
            var response = await client.PostAsync(this.GetUri(url), null);


            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);
        }

        public async Task<bool> UploadPictureAsync(Models.FileModel image)
        {
            if (image.IsValid == false)
                throw new ArgumentException("image is not valid", "image");

            var client = this.CreateClient();
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(image.Content), "file", image.FileName);
            var response = await client.PostAsync(this.GetUri(ApiUrls.PictureUrl), content);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);
        }

        public async Task<bool> DeletePictureAsync()
        {
            var client = this.CreateClient();
            var response = await client.DeleteAsync(this.GetUri(ApiUrls.PictureUrl));

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }
    }
}
