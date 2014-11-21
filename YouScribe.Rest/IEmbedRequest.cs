using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest
{
    public interface IEmbedRequest : IYouScribeRequest
    {
        /// <summary>
        /// Generate the embed iframe tag for a product
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns></returns>
        Task<string> GenerateIframeTagAsync(int id);

        /// <summary>
        ///  Generate the embed iframe tag for a product
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <param name="features">The feature of the embed (widht, height, ...)</param>
        /// <returns></returns>
        Task<string> GenerateIframeTagAsync(int id, EmbedGenerateModel features);

        /// <summary>
        /// Generate the embed iframe tag for a private product
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns></returns>
        Task<string> GeneratePrivateIframeTagAsync(int id);

        /// <summary>
        ///  Generate the embed iframe tag for a private product
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <param name="features">The feature of the embed (widht, height, ...)</param>
        /// <returns></returns>
        Task<string> GeneratePrivateIframeTagAsync(int id, PrivateEmbedGenerateModel features);
    }
}
