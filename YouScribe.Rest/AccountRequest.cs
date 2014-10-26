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
            var request = this.createRequest(ApiUrls.AccountUrl, Method.POST);
            request.AddBody(account);

            var response = client.Execute<AccountModel>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                await this.AddErrorsAsync(response);
                return null;
            }
            return response.Data;
        }

        public async Task<bool> UpdateAsync(Models.Accounts.AccountModel account)
        {
            var requesst = this.createRequest(ApiUrls.AccountUrl, Method.PUT);
            requesst.AddBody(account);

            var response = client.Execute(requesst);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<bool> SetSpokenLanguagesAsync(IEnumerable<string> languages)
        {
            var request = this.createRequest(ApiUrls.AccountLanguagesUrl, Method.PUT);

            if (languages != null)
            {
                foreach (var lng in languages)
                {
                    request.AddParameter("Languages", lng);
                }
            }

            var response = client.Execute(request);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<bool> UploadPictureAsync(Uri uri)
        {
            if (uri.IsValid() == false)
            {
                this.Errors.Add("uri invalid");
                return false;
            }
            var request = this.createRequest(ApiUrls.PictureUpdateUrl, Method.POST)
                .AddUrlSegment("url", uri.ToString())
                ;

            var response = client.Execute(request);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);
        }

        public async Task<bool> UploadPictureAsync(Models.FileModel image)
        {
            if (image.IsValid == false)
            {
                this.Errors.Add("invalid image parameters");
                return false;
            }

            var request = this.createRequest(ApiUrls.PictureUrl, Method.POST);

            using (MemoryStream ms = new MemoryStream())
            {
                image.Content.CopyTo(ms);

                var bytes = ms.ToArray();
                request.AddFile("file", bytes, image.FileName, image.ContentType);
            }
            
            var response = client.Execute(request);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);
        }

        public async Task<bool> DeletePictureAsync()
        {
            var request = this.createRequest(ApiUrls.PictureUrl, Method.DELETE);
            
            var response = client.Execute(request);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }
    }
}
