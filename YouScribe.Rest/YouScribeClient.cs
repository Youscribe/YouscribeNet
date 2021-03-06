﻿using System;
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
    public class ConcurentClient
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

    public class ClientsPoolProvider
    {
        public const int PoolMinSize = 10;
        public const int PoolIncrement = 5;

        public ClientsPoolProvider()
        {
            Clients = new List<ConcurentClient>();
            if (Clients.Count == 0)
            {
                lock (Clients)
                {
                    if (Clients.Count == 0)
                    {
                        for (int i = 0; i < PoolMinSize; i++)
                        {
                            Clients.Add(new ConcurentClient() { Reserved = 0, Client = null });
                        }
                    }
                }
            }
        }

        public List<ConcurentClient> Clients { get; private set; }

        static ClientsPoolProvider defaultPool = new ClientsPoolProvider();

        public static ClientsPoolProvider Default
        {
            get
            {
                return defaultPool;
            }
        }

        internal static void ClearDefaults()
        {
            defaultPool.Clients.Clear();
        }

    }

    public class YouScribeClient : IYouScribeClient
    {
        readonly ClientsPoolProvider poolProvider;

        const string defaultProductName = "YouScribe.Rest";

        internal readonly Func<DisposableClient> clientFactory;
        internal readonly Func<IYousScribeHttpClient> simpleClientFactory;
        internal readonly Func<HttpMessageHandler> httpMessageHandlerFactory;
        Func<Func<HttpMessageHandler>, IYousScribeHttpClient> baseClientFactory;

        private ITokenProvider _authorizeTokenProvider = new DefaultTokenProvider();

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
        
        static YouScribeClient()
        {
            DefaultTimeout = TimeSpan.FromMinutes(1);
        }

        public YouScribeClient(string baseUrl)
            : this(null, baseUrl)
        {
        }
        public YouScribeClient(string baseUrl, Func<Func<HttpMessageHandler>, IYousScribeHttpClient> baseClientFactory, ClientsPoolProvider poolProvider = null)
            : this(null, baseUrl, baseClientFactory, poolProvider)
        {
        }
        public YouScribeClient(Func<HttpMessageHandler> handlerFactory, string baseUrl, Func<Func<HttpMessageHandler>, IYousScribeHttpClient> baseClientFactory, ClientsPoolProvider poolProvider = null)
            : this(handlerFactory, baseUrl, baseClientFactory, TimeSpan.FromMinutes(15), poolProvider)
        {
        }

        public YouScribeClient()
            : this(ApiUrls.BaseUrl)
        { }

        public YouScribeClient(Func<Func<HttpMessageHandler>, IYousScribeHttpClient> baseClientFactory, ClientsPoolProvider poolProvider = null)
            : this(ApiUrls.BaseUrl, baseClientFactory, poolProvider)
        { }

        public YouScribeClient(Func<HttpMessageHandler> handlerFactory, ClientsPoolProvider poolProvider = null)
            : this(handlerFactory, ApiUrls.BaseUrl, poolProvider)
        { }
        public YouScribeClient(Func<HttpMessageHandler> handlerFactory, Func<Func<HttpMessageHandler>, IYousScribeHttpClient> baseClientFactory, ClientsPoolProvider poolProvider = null)
            : this(handlerFactory, ApiUrls.BaseUrl, baseClientFactory, poolProvider)
        { }

        public YouScribeClient(Func<HttpMessageHandler> handlerFactory, string baseUrl, ClientsPoolProvider poolProvider = null)
            : this(handlerFactory, baseUrl, null, DefaultTimeout, poolProvider)
        {
        }

        public YouScribeClient(Func<HttpMessageHandler> handlerFactory, string baseUrl, 
            Func<Func<HttpMessageHandler>, IYousScribeHttpClient> baseClientFactory, TimeSpan timeout, ClientsPoolProvider poolProvider = null)
        {
            this.BaseUrl = baseUrl;
            this.poolProvider = poolProvider ?? ClientsPoolProvider.Default;
            if (baseClientFactory == null)
            {
                this.baseClientFactory = (c) =>
                {
                    var client = c == null ? new HttpClient() : new HttpClient(c());
                    client.Timeout = timeout;

                    return new YousScribeHttpClient(client);
                };
            }
            else
            {
                this.baseClientFactory = baseClientFactory;
            }
            this.simpleClientFactory = () => this.baseClientFactory(handlerFactory);
            this.clientFactory = () =>
            {
                var client = this.ReserveClient(handlerFactory);
                if (client == null)
                {
                    var count = this.poolProvider.Clients.Count;
                    lock (this.poolProvider.Clients)
                    {
                        if (this.poolProvider.Clients.Count != count)
                            client = this.ReserveClient(handlerFactory);
                        if (client != null)
                            return client;
                        for (var i = 0; i < ClientsPoolProvider.PoolIncrement - 1; i++)
                        {
                            this.poolProvider.Clients.Add(new ConcurentClient() { Reserved = 0, Client = null });
                        }
                        var item = new ConcurentClient() { Reserved = 1, Client = this.simpleClientFactory() };
                        this.poolProvider.Clients.Add(item);
                        return new DisposableClient(item);
                    }
                }
                return client;
            };
        }

        internal DisposableClient ReserveClient(Func<HttpMessageHandler> handlerFactory)
        {
            for (var i = 0; i < poolProvider.Clients.Count; i++)
            {
                var item = poolProvider.Clients[i];
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
            _authorizeTokenProvider.SetToken(token);
        }

        public void SetTokenProvider(ITokenProvider tokenProvider)
        {
            _authorizeTokenProvider = tokenProvider;
        }

        public string GetToken()
        {
            return _authorizeTokenProvider.GetToken();
        }

        public ITokenProvider GetTokenProvider()
        {
            return _authorizeTokenProvider;
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

        public Func<DisposableClient> GetClientFactory()
        {
            return this.clientFactory;
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
                this.SetToken(token);

                return true;
            }
        }

        public IProductRequest CreateProductRequest()
        {
            var productRequest = new ProductRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return productRequest;
        }

        public ILibraryRequest CreateLibraryRequest()
        {
            var libraryRequest = new LibraryRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return libraryRequest;
        }

        public IAccountRequest CreateAccountRequest()
        {
            var request = new AccountRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IAccountEventRequest CreateAccountEventRequest()
        {
            var request = new AccountEventRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IAccountPublisherRequest CreateAccountPublisherRequest()
        {
            var request = new AccountPublisherRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IAccountUsertTypeRequest CreateAccountUserTypeRequest()
        {
            var request = new AccountUserTypeRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IAccountUtilRequest CreateAccountUtilRequest()
        {
            var request = new AccountUtilRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IEmbedRequest CreateEmbedRequest()
        {
            var request = new EmbedRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IProductCommentRequest CreateProductCommentRequest()
        {
            var request = new ProductCommentRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IProductSearchRequest CreateProductSearchRequest()
        {
            var request = new ProductSearchRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IProductSuggestRequest CreateProductSuggestRequest()
        {
            var request = new ProductSuggestRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IThemeRequest CreateThemeRequest()
        {
            var request = new ThemeRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return request;
        }

        public IPropertyRequest CreatePropertyRequest()
        {
            var request = new PropertyRequest(this.clientFactory, _authorizeTokenProvider) { BaseUrl = this.BaseUrl };
            return request;
        }
    }
}
