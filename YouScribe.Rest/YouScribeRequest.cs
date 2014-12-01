using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    class YouScribeRequest : IYouScribeRequest
    {
        protected readonly Func<HttpClient> clientFactory;
        protected readonly string authorizeToken;
        protected readonly ISerializer serializer = new JSonSerializer();

        public RequestError Error { get; internal set; }

        public string BaseUrl
        {
            private get;
            set;
        }

        public YouScribeRequest(Func<HttpClient> clientFactory, string authorizeToken)
        {
            this.BaseUrl = ApiUrls.BaseUrl;
            this.clientFactory = clientFactory;
            this.authorizeToken = authorizeToken;
            this.Error = new RequestError(this);
        }

        protected HttpClient CreateClient()
        {
            this.Error = new RequestError(this);
            var client = clientFactory();
            if (client.DefaultRequestHeaders.Contains(ApiUrls.AuthorizeTokenHeaderName))
                client.DefaultRequestHeaders.Remove(ApiUrls.AuthorizeTokenHeaderName);
            if (string.IsNullOrEmpty(this.authorizeToken) == false)
                client.DefaultRequestHeaders.Add(ApiUrls.AuthorizeTokenHeaderName, this.authorizeToken);

            return client;
        }

        protected HttpContent GetContent<T>(T obj)
        {
            var str = serializer.Serialize(obj);
            return new StringContent(str, Encoding.UTF8, "application/json");
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
            var str = await content.ReadAsStringAsync();
            return serializer.Deserialize<T>(str);
        }

        protected async Task AddErrorsAsync(HttpResponseMessage response)
        {
            var error = await response.Content.ReadAsStringAsync();
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

        protected async Task<bool> HandleResponseAsync(HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized 
                || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                this.Error.Messages = new List<string>(){ "Not connected" };
                this.Error.StatusCode = (int)response.StatusCode;
                return false;
            }
            else if (response.StatusCode != expectedStatusCode)
            {
                await this.AddErrorsAsync(response);
                return false;
            }
            return true;
        }
    }
}
