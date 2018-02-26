using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest
{
    public interface IProductSuggestRequest : IYouScribeRequest
    {
        Task<IEnumerable<ProductSuggestItemOutputModel>> GetSuggestSameOwnerAsync(int id, string offerType = null, string domainLanguage = "fr", int take = 3);
        Task<IEnumerable<ProductSuggestItemOutputModel>> GetSuggestAsync(int id, string offerType = null, string domainLanguage = "fr", int take = 3);
        Task<IEnumerable<ProductSuggestItemOutputModel>> GetSuggestSimilarDocumentsAsync(int id, string offerType = null, string domainLanguage = "fr", int take = 3);
    }
}
