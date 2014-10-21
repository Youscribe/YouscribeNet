using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    public class YouScribeClient : IYouScribeClient
    {
        internal readonly IRestClient client;

        private string _authorizeToken;

        public YouScribeClient()
            : this(ApiUrls.BaseUrl)
        { }

        public YouScribeClient(string baseUrl)
        {
            this.client = new RestClient(baseUrl);
            this.client.UserAgent = "YouScribe/" + this.GetType().Assembly.GetName().Version;
        }

        public void SetToken(string token)
        {
            _authorizeToken = token;
        }

        public string GetToken()
        {
            return _authorizeToken;
        }

        public void SetUserAgent(string userAgent)
        {
            this.client.UserAgent = userAgent;
        }

        public bool Authorize(string userNameOrEmail, string password)
        {
            var request = new RestRequest(ApiUrls.AuthorizeUrl, Method.POST)
                .AddParameter("UserName", userNameOrEmail)
                .AddParameter("Password", password)
                ;

            var response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return false;

            if (response.Headers.Any(c => c.Name == ApiUrls.AuthorizeTokenHeaderName) == false)
                return false;

            var token = response.Headers.Where(c => c.Name == ApiUrls.AuthorizeTokenHeaderName)
                .First()
                .Value
                .ToString()
                ;
            _authorizeToken = token;

            return true;
        }

        public IProductRequest CreateProductRequest()
        {
            var productRequest = new ProductRequest(this.client, _authorizeToken);
            return productRequest;
        }

        public ILibraryRequest CreateLibraryRequest()
        {
            var libraryRequest = new LibraryRequest(this.client, _authorizeToken);
            return libraryRequest;
        }
        
        public IAccountRequest CreateAccountRequest()
        {
            var request = new AccountRequest(this.client, _authorizeToken);
            return request;
        }
        
        public IAccountEventRequest CreateAccountEventRequest()
        {
            var request = new AccountEventRequest(this.client, _authorizeToken);
            return request;
        }

        public IAccountPublisherRequest CreateAccountPublisherRequest()
        {
            var request = new AccountPublisherRequest(this.client, _authorizeToken);
            return request;
        }

        public IAccountUsertTypeRequest CreateAccountUserTypeRequest()
        {
            var request = new AccountUserTypeRequest(this.client, _authorizeToken);
            return request;
        }

        public IAccountDeviceRequest CreateAccountDeviceRequest()
        {
            var request = new AccountDeviceRequest(this.client, _authorizeToken);
            return request;
        }

        public IEmbedRequest CreateEmbedRequest()
        {
            var request = new EmbedRequest(this.client, _authorizeToken);
            return request;
        }
    }
}
