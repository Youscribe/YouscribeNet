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
        public ProductSearchRequest(Func<DisposableClient> clientFactory, string token)
            : base(clientFactory, token)
        { }

        public async Task<ProductSearchOutputModel> SearchProductsAsync(ProductSearchInputModel input)
        {
            var model = await this.PostWithResult<ProductSearchOutputModel>(ApiUrls.ProductSearchUrl, input);
            if (model == null)
                return new ProductSearchOutputModel() { TotalResults = -1, Products = Enumerable.Empty<ProductSearchItemOutputModel>() };
            return model;
        }

        public async Task<ProductSearchOutputModel> SearchProductsAsyncV2(ProductSearchInputModel input)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
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
}
