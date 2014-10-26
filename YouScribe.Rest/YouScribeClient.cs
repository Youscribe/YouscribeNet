using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    public class YouScribeClient : IYouScribeClient
    {
        internal readonly Func<HttpClient> clientFactory;

        private string _authorizeToken;

        public YouScribeClient()
            : this(ApiUrls.BaseUrl)
        { }

        public YouScribeClient(string baseUrl)
        {
            this.clientFactory = () =>
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.Add(
                    new System.Net.Http.Headers.ProductInfoHeaderValue("YouScribe", this.GetType().Assembly.GetName().Version.ToString())
                    );
                return client;
            };
        }

        public void SetToken(string token)
        {
            _authorizeToken = token;
        }

        public string GetToken()
        {
            return _authorizeToken;
        }

        //public void SetUserAgent(string userAgent)
        //{
        //    this.client.UserAgent = userAgent;
        //}

        public async Task<bool> AuthorizeAsync(string userNameOrEmail, string password)
        {
            using (var client = this.clientFactory())
            {
                var content = new System.Net.Http.FormUrlEncodedContent(new []{ 
                    new KeyValuePair<string, string>("UserName", userNameOrEmail),
                    new KeyValuePair<string, string>("Password", password)
                });
                var response = await client.PostAsync(ApiUrls.AuthorizeUrl, content);
                if (!response.IsSuccessStatusCode)
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
        }

        public IProductRequest CreateProductRequest()
        {
            var productRequest = new ProductRequest(this.clientFactory, _authorizeToken);
            return productRequest;
        }

        public ILibraryRequest CreateLibraryRequest()
        {
            var libraryRequest = new LibraryRequest(this.clientFactory, _authorizeToken);
            return libraryRequest;
        }
        
        public IAccountRequest CreateAccountRequest()
        {
            var request = new AccountRequest(this.clientFactory, _authorizeToken);
            return request;
        }
        
        public IAccountEventRequest CreateAccountEventRequest()
        {
            var request = new AccountEventRequest(this.clientFactory, _authorizeToken);
            return request;
        }

        public IAccountPublisherRequest CreateAccountPublisherRequest()
        {
            var request = new AccountPublisherRequest(this.clientFactory, _authorizeToken);
            return request;
        }

        public IAccountUsertTypeRequest CreateAccountUserTypeRequest()
        {
            var request = new AccountUserTypeRequest(this.clientFactory, _authorizeToken);
            return request;
        }

        public IEmbedRequest CreateEmbedRequest()
        {
            var request = new EmbedRequest(this.clientFactory, _authorizeToken);
            return request;
        }
    }
}
