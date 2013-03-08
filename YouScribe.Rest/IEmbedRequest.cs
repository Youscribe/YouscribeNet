using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        string GenerateIframeTag(int id);

        /// <summary>
        ///  Generate the embed iframe tag for a product
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <param name="features">The feature of the embed (widht, height, ...)</param>
        /// <returns></returns>
        string GenerateIframeTag(int id, EmbedGenerateModel features);

        /// <summary>
        /// Generate the embed iframe tag for a private product
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns></returns>
        string GeneratePrivateIframeTag(int id);

        /// <summary>
        ///  Generate the embed iframe tag for a private product
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <param name="features">The feature of the embed (widht, height, ...)</param>
        /// <returns></returns>
        string GeneratePrivateIframeTag(int id, PrivateEmbedGenerateModel features);
    }
}
