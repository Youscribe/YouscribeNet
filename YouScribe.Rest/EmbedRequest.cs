using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace YouScribe.Rest
{
    class EmbedRequest : YouScribeRequest, IEmbedRequest
    {
        public EmbedRequest(Func<HttpClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

        public string GenerateIframeTag(int id)
        {
            return this.GenerateIframeTag(id, null);
        }

        public string GenerateIframeTag(int id, Models.Products.EmbedGenerateModel features)
        {
            var request = this.createRequest(ApiUrls.EmbedUrl, Method.GET)
                .AddUrlSegment("id", id.ToString())
                ;
            this.generateParameters(request, features);

            var response = client.Execute<YouScribe.Rest.Models.Products.EmbedResponse>(request);

            this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);

            return response.Data != null ? response.Data.Content : string.Empty;
        }
        

        public string GeneratePrivateIframeTag(int id)
        {
            return this.GeneratePrivateIframeTag(id, null);
        }

        public string GeneratePrivateIframeTag(int id, Models.Products.PrivateEmbedGenerateModel features)
        {
            var request = this.createRequest(ApiUrls.PrivateEmbedUrl, Method.GET)
                .AddParameter("id", id)
                ;
            this.generateParameters(request, features);

            var response = client.Execute<YouScribe.Rest.Models.Products.EmbedResponse>(request);

            this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK);

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
