using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace YouScribe.Rest
{
    class YouScribeRequest : IYouScribeRequest
    {
        protected readonly IRestClient client;
        protected readonly string authorizeToken;

        public ICollection<string> Errors
        {
            get;
            private set;
        }

        IEnumerable<string> IYouScribeRequest.Errors
        {
            get { return this.Errors; }
        }

        public YouScribeRequest(IRestClient client, string authorizeToken)
        {
            this.client = client;
            this.authorizeToken = authorizeToken;
            this.Errors = new List<string>();
        }

        protected IRestRequest createRequest(string url, Method method)
        {
            this.Errors.Clear();

            var request = new RestRequest(url, method);
            request.RequestFormat = DataFormat.Json;
            if (string.IsNullOrEmpty(this.authorizeToken) == false)
                request.AddHeader(ApiUrls.AuthorizeTokenHeaderName, this.authorizeToken);

            return request;
        }

        protected void addErrors(IRestResponse response)
        {
            var error = response.Content;
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

        protected bool handleResponse(IRestResponse response, System.Net.HttpStatusCode expectedStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                this.Errors.Add("Not connected");
                return false;
            }
            else if (response.StatusCode != expectedStatusCode)
            {
                this.addErrors(response);
                return false;
            }
            return true;
        }
    }
}
