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
        public EmbedRequest(Func<HttpClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

        public Task<string> GenerateIframeTagAsync(int id)
        {
            return this.GenerateIframeTagAsync(id, null);
        }

        public async Task<string> GenerateIframeTagAsync(int id, Models.Products.EmbedGenerateModel features)
        {
            var request = this.createRequest(ApiUrls.EmbedUrl, Method.GET)
                .AddUrlSegment("id", id.ToString())
                ;
            this.generateParameters(request, features);

            var response = client.Execute<YouScribe.Rest.Models.Products.EmbedResponse>(request);

            await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);

            return response.Data != null ? response.Data.Content : string.Empty;
        }


        public Task<string> GeneratePrivateIframeTagAsync(int id)
        {
            return this.GeneratePrivateIframeTagAsync(id, null);
        }

        public async Task<string> GeneratePrivateIframeTagAsync(int id, Models.Products.PrivateEmbedGenerateModel features)
        {
            var request = this.createRequest(ApiUrls.PrivateEmbedUrl, Method.GET)
                .AddParameter("id", id)
                ;
            this.generateParameters(request, features);

            var response = client.Execute<YouScribe.Rest.Models.Products.EmbedResponse>(request);

            await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);

            return response.Data != null ? response.Data.Content : string.Empty;
        }

        private void generateParameters(IRestRequest request, Models.Products.EmbedGenerateModel features)
        {
            if (features == null)
                return;
            if (features.DisplayMode.HasValue)
                request.AddParameter("displayMode", features.DisplayMode.Value);
            if (features.Height.HasValue)
                request.AddParameter("height", features.Height.Value);
            if (features.StartPage.HasValue)
                request.AddParameter("startPage", features.StartPage.Value);
            if (features.Width.HasValue)
                request.AddParameter("width", features.Width.Value);

        }

        private void generateParameters(IRestRequest request, Models.Products.PrivateEmbedGenerateModel features)
        {
            if (features == null)
                return;
            this.generateParameters(request, (Models.Products.EmbedGenerateModel)features);

            if (string.IsNullOrEmpty(features.AccessPeriod) == false)
                request.AddParameter("accessPeriod", features.AccessPeriod);
        }
    }
}
