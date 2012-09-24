using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace YouScribe.Rest
{
    public class YouScribeClient : IYouScribeClient
    {
        private readonly IRestClient client;

        private string _authorizeToken;

        public YouScribeClient()
            : this(ApiUrls.BaseUrl)
        { }

        public YouScribeClient(string baseUrl)
        {
            this.client = new RestClient(baseUrl);
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
    }
}
