﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    class ProductSuggestRequest : YouScribeRequest, IProductSuggestRequest
    {
        public ProductSuggestRequest(Func<DisposableClient> clientFactory, ITokenProvider authorizeTokenProvider) : 
            base(clientFactory, authorizeTokenProvider)
        {

        }

        public Task<IEnumerable<Models.Products.ProductSuggestItemOutputModel>> GetSuggestSameOwnerAsync(int id, string offerType = null, string domainLanguage = "fr", int take = 3)
        {
            var parameters = new Dictionary<string, string>();
            var dico = new Dictionary<string, string>(){
                { "domainLanguage", domainLanguage },
                { "take", take.ToString() },
                { "offerType", offerType }
            };

            var queryString = dico.ToQueryString();
            var url = "api/v1/products/{id}/suggests/sameowner".Replace("{id}", id.ToString());
            if (!string.IsNullOrEmpty(queryString))
                url = url + "?" + queryString;
            return this.GetEnumerableAsync<Models.Products.ProductSuggestItemOutputModel>(url);
        }

        public Task<IEnumerable<Models.Products.ProductSuggestItemOutputModel>> GetSuggestAsync(int id, string offerType = null, string domainLanguage = "fr", int take = 3)
        {
            var parameters = new Dictionary<string, string>();
            var dico = new Dictionary<string, string>(){
                { "domainLanguage", domainLanguage },
                { "take", take.ToString() },
                { "offerType", offerType }
            };

            var queryString = dico.ToQueryString();
            var url = "api/v2/products/{id}/suggests".Replace("{id}", id.ToString());
            if (!string.IsNullOrEmpty(queryString))
                url = url + "?" + queryString;
            return this.GetEnumerableAsync<Models.Products.ProductSuggestItemOutputModel>(url);
        }

        public async Task<IEnumerable<Models.Products.ProductSuggestItemOutputModel>> GetSuggestSimilarDocumentsAsync(int id, string offerType = null, string domainLanguage = "fr", int take = 3)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var parameters = new Dictionary<string, string>();
                var dico = new Dictionary<string, string>(){
                { "domainLanguage", domainLanguage },
                { "take", take.ToString() },
                { "offerType", offerType }
            };

                var queryString = dico.ToQueryString();
                var url = "api/v2/products/{id}/suggests/similar".Replace("{id}", id.ToString());
                if (!string.IsNullOrEmpty(queryString))
                    url = url + "?" + queryString;
                var response = await client.GetAsync(this.GetUri(url)).ConfigureAwait(false);

                await this.HandleResponseAsync(response).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                    return (await this.GetObjectAsync<IEnumerable<Models.Products.ProductSuggestItemOutputModel>>(response.Content).ConfigureAwait(false));
                return Enumerable.Empty<Models.Products.ProductSuggestItemOutputModel>();
            }
        }
    }
}
