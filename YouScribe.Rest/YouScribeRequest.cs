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

        public ICollection<string> Errors
        {
            get;
            private set;
        }

        IEnumerable<string> IYouScribeRequest.Errors
        {
            get { return this.Errors; }
        }

        public string BaseUrl
        {
            private get;
            set;
        }

        public YouScribeRequest(Func<HttpClient> clientFactory, string authorizeToken)
        {
            this.clientFactory = clientFactory;
            this.authorizeToken = authorizeToken;
            this.Errors = new List<string>();
        }

        protected HttpClient CreateClient()
        {
            this.Errors.Clear();
            var client = clientFactory();
            if (!string.IsNullOrEmpty(this.BaseUrl))
                client.BaseAddress = new Uri(this.BaseUrl);
            if (string.IsNullOrEmpty(this.authorizeToken) == false)
                client.DefaultRequestHeaders.Add(ApiUrls.AuthorizeTokenHeaderName, this.authorizeToken);

            return client;
        }

        protected HttpContent GetContent<T>(T obj)
        {
            var str = serializer.Serialize(obj);
            return new StringContent(str, Encoding.UTF8, "application/json");
        }

        protected async Task<T> GetObjectAsync<T>(HttpContent content)
        {
            var str = await content.ReadAsStringAsync();
            return serializer.Deserialize<T>(str);
        }

        protected async Task AddErrorsAsync(HttpResponseMessage response)
        {
            var error = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(error) == false)
            {
                if (error.StartsWith("[") && error.EndsWith("]"))
                {
                    var errors = error.Substring(1, error.Length - 2)
                        .Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(c => c.Trim(new[] { '\'' }));

                    foreach (var item in errors)
                        this.Errors.Add(item);
                }
                else
                    this.Errors.Add(error.Trim(new[] { '\'' }));
            }
        }

        protected async Task<bool> HandleResponseAsync(HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                this.Errors.Add("Not connected");
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
