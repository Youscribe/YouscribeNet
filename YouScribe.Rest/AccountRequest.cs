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

        public async Task<Models.Accounts.AccountModel> CreateAsync(Models.Accounts.AccountModel account)
        {
            using (var client = this.CreateClient())
            {
                var content = this.GetContent(account);
                var response = await client.PostAsync(ApiUrls.AccountUrl, content);

                if (response.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    await this.AddErrorsAsync(response);
                    return null;
                }
                return await this.GetObjectAsync<AccountModel>(response.Content);
            }
        }

        public async Task<bool> UpdateAsync(Models.Accounts.AccountModel account)
        {
            using (var client = this.CreateClient())
            {
                var content = this.GetContent(account);
                var response = await client.PutAsync(ApiUrls.AccountUrl, content);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }

        public async Task<bool> SetSpokenLanguagesAsync(IEnumerable<string> languages)
        {
            using (var client = this.CreateClient())
            {
                var content = new FormUrlEncodedContent(
                    languages.Select(c => new KeyValuePair<string, string>("Languages", c))
                );
                var response = await client.PutAsync(ApiUrls.AccountLanguagesUrl, content);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }

        public async Task<bool> UploadPictureAsync(Uri uri)
        {
            if (uri.IsValid() == false)
            {
                this.Errors.Add("uri invalid");
                return false;
            }
            using (var client = this.CreateClient())
            {
                var content = new FormUrlEncodedContent(new[]{
                    new KeyValuePair<string, string>("url", uri.ToString())
                });
                var response = await client.PostAsync(ApiUrls.PictureUrl, content);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);
            }
        }

        public async Task<bool> UploadPictureAsync(Models.FileModel image)
        {
            if (image.IsValid == false)
            {
                this.Errors.Add("invalid image parameters");
                return false;
            }

            using (var client = this.CreateClient())
            {
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(image.Content), Path.GetFileNameWithoutExtension(image.FileName), image.FileName);
                var response = await client.PostAsync(ApiUrls.PictureUrl, content);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);
            }
        }

        public async Task<bool> DeletePictureAsync()
        {
            using (var client = this.CreateClient())
            {
                var response = await client.DeleteAsync(ApiUrls.PictureUrl);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }
    }
}
