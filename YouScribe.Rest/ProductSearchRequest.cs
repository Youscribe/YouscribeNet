using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest
{
    class ProductSearchRequest : YouScribeRequest, IProductSearchRequest
    {
        public ProductSearchRequest(Func<HttpClient> clientFactory, string token)
            : base(clientFactory, token)
        { }

        public async Task<ProductSearchOutputModel> SearchProductsAsync(ProductSearchInputModel input)
        {
            using (var client = this.CreateClient())
            {
                var url = ApiUrls.ProductSearchUrl;
                var qs = input.ToQueryString();
                url = url + "?" + qs;

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    await this.AddErrorsAsync(response);
                    return new ProductSearchOutputModel() { TotalResults = -1, Products = Enumerable.Empty<ProductSearchItemOutputModel>() };
                }
                return await this.GetObjectAsync<ProductSearchOutputModel>(response.Content);
            }
        }
    }
}
