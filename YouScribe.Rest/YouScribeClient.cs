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
    internal class ConcurentClient
    {
        public IYousScribeHttpClient Client;

        public int Reserved;
    }


    public class DisposableClient : IDisposable
    {
        ConcurentClient client;

        public IYousScribeHttpClient Client
        {
            get { return client.Client; }
        }

        internal DisposableClient(ConcurentClient client)
        {
            this.client = client;
        }

        public void Dispose()
        {
            Interlocked.Decrement(ref client.Reserved);
        }
    }

    public class YouScribeClient : IYouScribeClient
    {

        static List<ConcurentClient> clients = new List<ConcurentClient>();
        const string defaultProductName = "YouScribe.Rest";

        internal readonly Func<DisposableClient> clientFactory;
        internal readonly Func<HttpMessageHandler> httpMessageHandlerFactory;
        Func<Func<HttpMessageHandler>, IYousScribeHttpClient> baseClientFactory;

        private string _authorizeToken;

        const int PoolMinSize = 10;
        const int PoolIncrement = 5;

        private List<ProductInfoHeaderValue> userAgents = new List<ProductInfoHeaderValue>()
        {
		    new ProductInfoHeaderValue(defaultProductName, "2.2")
	    };

        public string BaseUrl
        {
            set;
            get;
        }

        public static TimeSpan DefaultTimeout
        {
            get;
            set;
        }

        internal static void ClearClients()
        {
            clients.Clear();
        }

        static YouScribeClient()
        {
            DefaultTimeout = TimeSpan.FromMinutes(1);
            if (clients.Count == 0)
            {
                lock (clients)
                {
                    if (clients.Count == 0)
                    {
                        for (int i = 0; i < PoolMinSize; i++)
                        {
                            clients.Add(new ConcurentClient() { Reserved = 0, Client = null });
                        }
                    }
                }
            }
        }

        public YouScribeClient(string baseUrl)
            : this(null, baseUrl)
        {
        }
        public YouScribeClient(string baseUrl, Func<Func<HttpMessageHandler>, IYousScribeHttpClient> baseClientFactory)
            : this(null, baseUrl, baseClientFactory)
        {
        }
        public YouScribeClient(Func<HttpMessageHandler> handlerFactory, string baseUrl, Func<Func<HttpMessageHandler>, IYousScribeHttpClient> baseClientFactory)
            : this(handlerFactory, baseUrl, baseClientFactory, TimeSpan.FromMinutes(15))
        {
        }

        public YouScribeClient()
            : this(ApiUrls.BaseUrl)
        { }

        public YouScribeClient(Func<Func<HttpMessageHandler>, IYousScribeHttpClient> baseClientFactory)
            : this(ApiUrls.BaseUrl, baseClientFactory)
        { }

        public YouScribeClient(Func<HttpMessageHandler> handlerFactory)
            : this(handlerFactory, ApiUrls.BaseUrl)
        { }
        public YouScribeClient(Func<HttpMessageHandler> handlerFactory, Func<Func<HttpMessageHandler>, IYousScribeHttpClient> baseClientFactory)
            : this(handlerFactory, ApiUrls.BaseUrl, baseClientFactory)
        { }

        public YouScribeClient(Func<HttpMessageHandler> handlerFactory, string baseUrl)
            : this(handlerFactory, baseUrl, null, DefaultTimeout)
        {
        }

        public YouScribeClient(Func<HttpMessageHandler> handlerFactory, string baseUrl, Func<Func<HttpMessageHandler>, IYousScribeHttpClient> baseClientFactory, TimeSpan timeout)
        {
            this.BaseUrl = baseUrl;
            if (baseClientFactory == null)
            {
                this.baseClientFactory = (c) =>
                {
                    return c == null ? new YousScribeHttpClient(new HttpClient()) : new YousScribeHttpClient(new HttpClient(c()));
                };
            }
            else
            {
                this.baseClientFactory = baseClientFactory;
            }
            this.clientFactory = () =>
            {
                var client = this.ReserveClient(handlerFactory);
                if (client == null)
                {
                    var count = clients.Count;
                    lock (clients)
                    {
                        if (clients.Count != count)
                            client = this.ReserveClient(handlerFactory);
                        if (client != null)
                            return client;
                        for (var i = 0; i < PoolIncrement - 1; i++)
                        {
                            clients.Add(new ConcurentClient() { Reserved = 0, Client = null });
                        }
                        var item = new ConcurentClient() { Reserved = 1, Client = this.baseClientFactory(handlerFactory) };
                        clients.Add(item);
                        return new DisposableClient(item);
                    }
                }
                return client;
            };
        }

        public DisposableClient ReserveClient(Func<HttpMessageHandler> handlerFactory)
        {
            for (var i = 0; i < clients.Count; i++)
            {
                var item = clients[i];
                if (item.Reserved == 0)
                {
                    if (Interlocked.Increment(ref item.Reserved) == 1)
                    {
                        if (item.Client == null)
                            item.Client = this.baseClientFactory(handlerFactory);
                        var cclient = item.Client;
                        cclient.DefaultRequestHeaders.Clear();
                        cclient.DefaultRequestHeaders.UserAgent.Clear();
                        cclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        foreach (var userAgent in userAgents)
                            cclient.DefaultRequestHeaders.UserAgent.Add(userAgent);

                        return new DisposableClient(item);
                    }
                    else
                        Interlocked.Decrement(ref item.Reserved);
                }
            }
            return null;
        }

        public void SetToken(string token)
        {
            _authorizeToken = token;
        }

        public string GetToken()
        {
            return _authorizeToken;
        }

        public void AddUserAgent(string productName, string version)
        {
            var idx = 0;
            var last = this.userAgents.LastOrDefault();
            if (last != null && last.Product.Name == defaultProductName)
                idx = this.userAgents.Count - 1;
            this.userAgents.Insert(idx, new ProductInfoHeaderValue(productName, version));
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
            using (var dclient = this.clientFactory())
            {
                var client = dclient.Client;
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

        public IThemeRequest CreateThemeRequest()
        {
            var request = new ThemeRequest(this.clientFactory, _authorizeToken) { BaseUrl = this.BaseUrl };
            return request;
        }
    }
}
