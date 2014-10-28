using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest
{
    public interface IProductCommentRequest : IYouScribeRequest
    {
        /// <summary>
        /// Get comments for product
        /// </summary>
        /// <param name="productId">product identifier</param>
        /// <param name="skip">number of comments to skip</param>
        /// <param name="take">numer of comments to take</param>
        /// <param name="repliesTake">number of replies to take</param>
        /// <returns>Product list and count</returns>
        Task<ProductCommentsOutput> GetCommentsAsync(int productId, int skip = 0, int take = 5, int repliesTake = 3);
    }
}
