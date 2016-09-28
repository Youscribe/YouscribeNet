using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    class EmbedRequest : YouScribeRequest, IEmbedRequest
    {
        public EmbedRequest(Func<DisposableClient> clientFactory, ITokenProvider authorizeTokenProvider)
            : base(clientFactory, authorizeTokenProvider)
        { }

        public Task<string> GenerateIframeTagAsync(int id)
        {
            return this.GenerateIframeTagAsync(id, null);
        }

        public async Task<string> GenerateIframeTagAsync(int id, Models.Products.EmbedGenerateModel features)
        {
            var parameters = new Dictionary<string, string>();
            this.generateParameters(parameters, features);

            var queryString = parameters.ToQueryString();
            var url = ApiUrls.EmbedUrl.Replace("{id}", id.ToString());
            if (!string.IsNullOrEmpty(queryString))
                url = url + "?" + queryString;
            var model = await this.GetAsync<Models.Products.EmbedResponse>(url);
            if (model == null)
                return string.Empty;
            return model.Content;
        }


        public Task<string> GeneratePrivateIframeTagAsync(int id)
        {
            return this.GeneratePrivateIframeTagAsync(id, null);
        }

        public async Task<string> GeneratePrivateIframeTagAsync(int id, Models.Products.PrivateEmbedGenerateModel features)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("id", id.ToString());
            this.generateParameters(parameters, features);

            var queryString = parameters.ToQueryString();
            var url = ApiUrls.PrivateEmbedUrl;
            if (!string.IsNullOrEmpty(queryString))
                url = url + "?" + queryString;
            var model = await this.GetAsync<Models.Products.EmbedResponse>(url);
            if (model == null)
                return string.Empty;
            return model.Content;
        }

        private void generateParameters(IDictionary<string, string> parameters, Models.Products.EmbedGenerateModel features)
        {
            if (features == null)
                return;
            if (features.DisplayMode.HasValue)
                parameters.Add("displayMode", features.DisplayMode.Value.ToString());
            if (features.Height.HasValue)
                parameters.Add("height", features.Height.Value.ToString());
            if (features.StartPage.HasValue)
                parameters.Add("startPage", features.StartPage.Value.ToString());
            if (features.Width.HasValue)
                parameters.Add("width", features.Width.Value.ToString());

        }

        private void generateParameters(IDictionary<string, string> parameters, Models.Products.PrivateEmbedGenerateModel features)
        {
            if (features == null)
                return;
            this.generateParameters(parameters, (Models.Products.EmbedGenerateModel)features);

            if (string.IsNullOrEmpty(features.AccessPeriod) == false)
                parameters.Add(new KeyValuePair<string, string>("accessPeriod", features.AccessPeriod));
        }
    }
}
