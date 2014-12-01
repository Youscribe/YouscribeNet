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
		internal readonly Func<HttpMessageHandler> httpMessageHandlerFactory;

        private string _authorizeToken;

        private List<ProductInfoHeaderValue> userAgents = new List<ProductInfoHeaderValue>()
        {
            new ProductInfoHeaderValue("YouScribe", "2.0")
        };

        public YouScribeClient()
            : this(ApiUrls.BaseUrl)
        { }

        public YouScribeClient(string baseUrl)
            : this(null, baseUrl)
        { }

		public YouScribeClient(Func<HttpMessageHandler> handlerFactory)
            : this(handlerFactory, ApiUrls.BaseUrl)
        { }

			public YouScribeClient(Func<HttpMessageHandler> handlerFactory, string baseUrl)
        {
			this.httpMessageHandlerFactory = handlerFactory;
            this.clientFactory = () =>
            {
				var client = httpMessageHandlerFactory == null ? new HttpClient() : new HttpClient(httpMessageHandlerFactory());
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.Clear();
                foreach (var userAgent in userAgents)
                    client.DefaultRequestHeaders.UserAgent.Add(userAgent);
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

        public void SetUserAgent(string productName, string version)
        {
            this.userAgents = new List<ProductInfoHeaderValue>()
            {
                new ProductInfoHeaderValue(productName, version)
            };
        }

        public async Task<bool> AuthorizeAsync(string userNameOrEmail, string password, int? validityInHours = null)
        {
            using (var client = this.clientFactory())
            {
                var keyValues = new List<KeyValuePair<string, string>> { 
                    new KeyValuePair<string, string>("UserName", userNameOrEmail),
                    new KeyValuePair<string, string>("Password", password)
                };
                if (validityInHours.HasValue)
                    keyValues.Add(new KeyValuePair<string, string>("ValidityInHours", validityInHours.Value.ToString()));
                var content = new System.Net.Http.FormUrlEncodedContent(keyValues);
                var response = await client.PostAsync(ApiUrls.AuthorizeUrl, content);
                if (!response.IsSuccessStatusCode)
                    return false;
                if (response.Headers.Any(c => c.Key == ApiUrls.AuthorizeTokenHeaderName) == false)
                    return false;

                var token = response.Headers.Where(c => c.Key == ApiUrls.AuthorizeTokenHeaderName)
                .First()
                .Value.First()
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

        public IAccountUtilRequest CreateAccountUtilRequest()
        {
            var request = new AccountUtilRequest(this.clientFactory, _authorizeToken);
            return request;
        }

        public IEmbedRequest CreateEmbedRequest()
        {
            var request = new EmbedRequest(this.clientFactory, _authorizeToken);
            return request;
        }

        public IProductCommentRequest CreateProductCommentRequest()
        {
            var request = new ProductCommentRequest(this.clientFactory, _authorizeToken);
            return request;
        }

        public IProductSearchRequest CreateProductSearchRequest()
        {
            var request = new ProductSearchRequest(this.clientFactory, _authorizeToken);
            return request;
        }

        public IProductSuggestRequest CreateProductSuggestRequest()
        {
            var request = new ProductSuggestRequest(this.clientFactory, _authorizeToken);
            return request;
        }
    }
}
