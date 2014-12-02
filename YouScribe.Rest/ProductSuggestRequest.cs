using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    class ProductSuggestRequest : YouScribeRequest, IProductSuggestRequest
    {
        public ProductSuggestRequest(Func<HttpClient> clientFactory, string authorizeToken) : 
            base(clientFactory, authorizeToken)
        {

        }

        public async Task<IEnumerable<Models.Products.ProductSearchItemOutputModel>> GetSuggestAsync(int id, string offerType = null, string domainLanguage = "fr", int take = 3)
        {
            var client = this.CreateClient();
            var parameters = new Dictionary<string, string>();
            var dico = new Dictionary<string, string>(){
                { "domainLanguage", domainLanguage },
                { "take", take.ToString() },
                { "offerType", offerType }
            };

            var queryString = dico.ToQueryString();
            var url = "api/v1/products/{id}/suggests".Replace("{id}", id.ToString());
            if (!string.IsNullOrEmpty(queryString))
                url = url + "?" + queryString;
            var response = await client.GetAsync(this.GetUri(url)).ConfigureAwait(false);

            await this.HandleResponseAsync(response, System.Net.HttpStatusCode.OK).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                return (await this.GetObjectAsync<IEnumerable<Models.Products.ProductSearchItemOutputModel>>(response.Content).ConfigureAwait(false));
            return Enumerable.Empty<Models.Products.ProductSearchItemOutputModel>();
        }
    }
}
