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
        public ProductSearchRequest(Func<IYousScribeHttpClient> clientFactory, string token)
            : base(clientFactory, token)
        { }

        public async Task<ProductSearchOutputModel> SearchProductsAsync(ProductSearchInputModel input)
        {
            var client = this.CreateClient();
            var url = ApiUrls.ProductSearchUrl;

            var content = this.GetContent(input);
            var response = await client.PostAsync(this.GetUri(url), content).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                await this.AddErrorsAsync(response).ConfigureAwait(false);
                return new ProductSearchOutputModel() { TotalResults = -1, Products = Enumerable.Empty<ProductSearchItemOutputModel>() };
            }
            return await this.GetObjectAsync<ProductSearchOutputModel>(response.Content).ConfigureAwait(false);
        }

        public async Task<ProductSearchOutputModel> SearchProductsAsyncV2(ProductSearchInputModel input)
        {
            var client = this.CreateClient();
            var url = ApiUrls.ProductSearchUrlV2;

            var content = this.GetContent(input);
            var response = await client.PostAsync(this.GetUri(url), content).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                await this.AddErrorsAsync(response).ConfigureAwait(false);
                return new ProductSearchOutputModel() { TotalResults = -1, Products = Enumerable.Empty<ProductSearchItemOutputModel>() };
            }
            return await this.GetObjectAsync<ProductSearchOutputModel>(response.Content).ConfigureAwait(false);
        }
    }
}
