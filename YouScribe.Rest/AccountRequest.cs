using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Accounts;
using YouScribe.Rest.Helpers;
using System.IO;
using System.Net.Http;

namespace YouScribe.Rest
{
    class AccountRequest : YouScribeRequest, IAccountRequest
    {
        public AccountRequest(Func<HttpClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

        public Models.Accounts.AccountModel Create(Models.Accounts.AccountModel account)
        {
            var request = this.createRequest(ApiUrls.AccountUrl, Method.POST);
            request.AddBody(account);

            var response = client.Execute<AccountModel>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                this.addErrors(response);
                return null;
            }
            return response.Data;
        }

        public bool Update(Models.Accounts.AccountModel account)
        {
            var requesst = this.createRequest(ApiUrls.AccountUrl, Method.PUT);
            requesst.AddBody(account);

            var response = client.Execute(requesst);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }

        public bool SetSpokenLanguages(IEnumerable<string> languages)
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

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }

        public bool UploadPicture(Uri uri)
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

            return this.handleResponse(response, System.Net.HttpStatusCode.OK);
        }

        public bool UploadPicture(Models.FileModel image)
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

            return this.handleResponse(response, System.Net.HttpStatusCode.OK);
        }

        public bool DeletePicture()
        {
            var request = this.createRequest(ApiUrls.PictureUrl, Method.DELETE);
            
            var response = client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }
    }
}
