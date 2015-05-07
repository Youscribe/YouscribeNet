using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest
{
    public interface IProductSearchRequest : IYouScribeRequest
    {
        Task<ProductSearchOutputModel> SearchProductsAsync(ProductSearchInputModel input);

        Task<ProductSearchOutputModel> SearchProductsAsyncV2(ProductSearchInputModel input);
    }
}
