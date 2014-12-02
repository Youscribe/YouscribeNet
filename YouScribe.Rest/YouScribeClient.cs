using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    public class YouScribeClient : IYouScribeClient
    {
        static Dictionary<int, HttpClient> clients = new Dictionary<int,HttpClient>();

        internal readonly Func<HttpClient> clientFactory;
		internal readonly Func<HttpMessageHandler> httpMessageHandlerFactory;

        private string _authorizeToken;

        private List<ProductInfoHeaderValue> userAgents = new List<ProductInfoHeaderValue>()
        {
            new ProductInfoHeaderValue("YouScribe", "2.0")
        };

        public string BaseUrl
        {
            set;
            get;
        }

        public YouScribeClient(string baseUrl)
            : this(null, baseUrl)
        { }

	    public YouScribeClient()
            : this(ApiUrls.BaseUrl)
        { }

        public YouScribeClient(Func<HttpMessageHandler> handlerFactory)
            : this(handlerFactory, ApiUrls.BaseUrl)
        {
        }

        public YouScribeClient(Func<HttpMessageHandler> handlerFactory, string baseUrl)
        {
            this.BaseUrl = baseUrl;
            this.clientFactory = () =>
            {
                var id = System.Threading.Thread.CurrentThread.ManagedThreadId;

                if (!clients.ContainsKey(id))
                {
                    lock (clients)
                    {
                        if (!clients.ContainsKey(id))
                        {
                            var newDico = clients.ToDictionary(c => c.Key, c => c.Value);
                            var client = handlerFactory == null ? new HttpClient() : new HttpClient(handlerFactory());
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            newDico.Add(id, client);
                            Interlocked.Exchange(ref clients, newDico);
                        }
                    }
                }

                var cclient = clients[id];
                cclient.DefaultRequestHeaders.UserAgent.Clear();

                foreach (var userAgent in userAgents)
                    cclient.DefaultRequestHeaders.UserAgent.Add(userAgent);
                return cclient;
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
            var client = this.clientFactory();
            var keyValues = new List<KeyValuePair<string, string>> { 
                new KeyValuePair<string, string>("UserName", userNameOrEmail),
                new KeyValuePair<string, string>("Password", password)
            };
            if (validityInHours.HasValue)
                keyValues.Add(new KeyValuePair<string, string>("ValidityInHours", validityInHours.Value.ToString()));
            var content = new System.Net.Http.FormUrlEncodedContent(keyValues);
            var response = await client.PostAsync(new Uri(new Uri(BaseUrl), ApiUrls.AuthorizeUrl), content).ConfigureAwait(false);
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

        public IProductRequest CreateProductRequest()
        {
            var productRequest = new ProductRequest(this.clientFactory, _authorizeToken) { BaseUrl = this.BaseUrl };
            return productRequest;
        }

        public ILibraryRequest CreateLibraryRequest()
        {
            var libraryRequest = new LibraryRequest(this.clientFactory, _authorizeToken) { BaseUrl = this.BaseUrl };
            return libraryRequest;
		}
        
        public IAccountRequest CreateAccountRequest()
        {
            var request = new AccountRequest(this.clientFactory, _authorizeToken) { BaseUrl = this.BaseUrl };
            return request;
        }
        
        public IAccountEventRequest CreateAccountEventRequest()
        {
            var request = new AccountEventRequest(this.clientFactory, _authorizeToken) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IAccountPublisherRequest CreateAccountPublisherRequest()
        {
            var request = new AccountPublisherRequest(this.clientFactory, _authorizeToken) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IAccountUsertTypeRequest CreateAccountUserTypeRequest()
        {
            var request = new AccountUserTypeRequest(this.clientFactory, _authorizeToken) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IAccountUtilRequest CreateAccountUtilRequest()
        {
            var request = new AccountUtilRequest(this.clientFactory, _authorizeToken) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IEmbedRequest CreateEmbedRequest()
        {
            var request = new EmbedRequest(this.clientFactory, _authorizeToken) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IProductCommentRequest CreateProductCommentRequest()
        {
            var request = new ProductCommentRequest(this.clientFactory, _authorizeToken) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IProductSearchRequest CreateProductSearchRequest()
        {
            var request = new ProductSearchRequest(this.clientFactory, _authorizeToken) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IProductSuggestRequest CreateProductSuggestRequest()
        {
            var request = new ProductSuggestRequest(this.clientFactory, _authorizeToken) { BaseUrl = this.BaseUrl };
            return request;
        }
    }
}
