using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    public class YouScribeRequest : IYouScribeRequest
    {
        protected readonly Func<DisposableClient> clientFactory;
        protected readonly ISerializer serializer = new JSonSerializer();
        protected readonly IDictionary<string, IEnumerable<string>> headers = new Dictionary<string, IEnumerable<string>>();

        protected string authorizeToken;

        public RequestError Error { get; internal set; }

        public string BaseUrl
        {
            private get;
            set;
        }

        public YouScribeRequest(Func<DisposableClient> clientFactory, string authorizeToken)
        {
            this.BaseUrl = ApiUrls.BaseUrl;
            this.clientFactory = clientFactory;
            this.authorizeToken = authorizeToken;
            this.Error = new RequestError(this);
        }

        protected DisposableClient CreateClient()
        {
            this.Error = new RequestError(this);
            var dclient = clientFactory();
            var client = dclient.Client;
            if (string.IsNullOrEmpty(this.authorizeToken) == false)
                client.DefaultRequestHeaders.Add(ApiUrls.AuthorizeTokenHeaderName, this.authorizeToken);
            foreach (var header in headers)
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            return dclient;
        }

        protected HttpContent GetContent<T>(T obj)
        {
            var str = serializer.Serialize(obj);
            return new StringContent(str, Encoding.UTF8, "application/json");
        }

        public void SetToken(string authorizeToken)
        {
            this.authorizeToken = authorizeToken;
        }

        public string GetToken()
        {
            return this.authorizeToken;
        }

        public Uri GetUri(string relativeUri)
        {
            return new Uri(new Uri(BaseUrl), relativeUri);
        }

        public T GetObject<T>(string content)
        {
            return serializer.Deserialize<T>(content);
        }

        protected async Task<T> GetObjectAsync<T>(HttpContent content)
        {
            var str = await content.ReadAsStringAsync().ConfigureAwait(false);
            return serializer.Deserialize<T>(str);
        }

        protected async Task AddErrorsAsync(HttpResponseMessage response)
        {
            var error = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var errorMessages = new List<string>();
            if (string.IsNullOrEmpty(error) == false)
            {
                if (error.StartsWith("[") && error.EndsWith("]"))
                {
                    var errors = error.Substring(1, error.Length - 2)
                        .Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(c => c.Trim(new[] { '\'' }));

                    foreach (var item in errors)
                        errorMessages.Add(item);
                }
                else
                    errorMessages.Add(error.Trim(new[] { '\'' }));
            }
            this.Error.Messages = errorMessages;
            this.Error.StatusCode = (int)response.StatusCode;
            this.Error.RawOutput = error;
        }

        protected async Task<bool> HandleResponseAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized 
                || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                this.Error.Messages = new List<string>(){ "Not connected/authorized" };
                this.Error.StatusCode = (int)response.StatusCode;
                return false;
            }
            else if (!response.IsSuccessStatusCode)
            {
                await this.AddErrorsAsync(response).ConfigureAwait(false);
                return false;
            }
            return true;
        }

        public void AddHeader(string name, IEnumerable<string> values)
        {
            this.headers.Add(name, values);
        }

        public void AddHeader(string name, string value)
        {
            this.headers.Add(name, new []{ value });
        }

        public void SetHeader(string name, string value)
        {
            this.headers[name] = new[] { value };
        }

        public async Task<T> GetAsync<T>(string url)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var response = await client.GetAsync(this.GetUri(url)).ConfigureAwait(false);

                if (!(await this.HandleResponseAsync(response).ConfigureAwait(false)))
                    return default(T);
                var result = await this.GetObjectAsync<T>(response.Content).ConfigureAwait(false);
                return result;
            }
        }

        public async Task<IEnumerable<T>> GetEnumerableAsync<T>(string url)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var response = await client.GetAsync(this.GetUri(url)).ConfigureAwait(false);

                if (!(await this.HandleResponseAsync(response).ConfigureAwait(false)))
                    return Enumerable.Empty<T>();
                var result = await this.GetObjectAsync<IEnumerable<T>>(response.Content).ConfigureAwait(false);
                return result;
            }
        }

        public async Task PostAsync(string url, object input)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;

                var content = this.GetContent(input);
                var response = await client.PostAsync(this.GetUri(url), content).ConfigureAwait(false);

                await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<T> PostWithResultAsync<T>(string url, object input)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;

                var content = this.GetContent(input);
                var response = await client.PostAsync(this.GetUri(url), content).ConfigureAwait(false);

                if (!(await this.HandleResponseAsync(response).ConfigureAwait(false)))
                    return default(T);
                return await this.GetObjectAsync<T>(response.Content).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> PostWithEnumerableResultAsync<T>(string url, object input)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;

                var content = this.GetContent(input);
                var response = await client.PostAsync(this.GetUri(url), content).ConfigureAwait(false);

                if (!(await this.HandleResponseAsync(response).ConfigureAwait(false)))
                    return Enumerable.Empty<T>();
                return await this.GetObjectAsync<IEnumerable<T>>(response.Content).ConfigureAwait(false);
            }
        }

        public void AssertNoError()
        {
            if (this.Error.Messages.Any())
                throw new Exception(string.Join(Environment.NewLine, this.Error.Messages));
        }
    }
}
