using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Libraries;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    public interface ILibraryRequest : IYouScribeRequest
    {
		/// <summary>
		/// List all libraries for current user
		/// </summary>
		/// <returns>List of libraries</returns>
		Task<IEnumerable<SimpleLibraryModel>> GetAsync ();

		/// <summary>
		/// Get library by id
		/// </summary>
		/// <param name="id">library id</param>
		/// <returns>Library</returns>
		Task<Models.Libraries.LibraryModel> GetAsync (int id);

        /// <summary>
        /// Get library by typeName
        /// </summary>
        /// <param name="typeName">library type name</param>
        /// <returns>Library</returns>
        Task<LibraryModel> GetAsync(string typeName);

        /// <summary>
        /// Add product to library
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<bool> AddProductAsync(int id, int productId, bool isPublic);

        /// <summary>
        /// Add product to library
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<bool> AddProductAsync(string typeName, int productId, bool isPublic);


        /// <summary>
        /// Add product to custom library
        /// </summary>
        /// <param name="libraryLabel"></param>
        /// <param name="productId"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        Task<bool> AddProductInCustomLibraryAsync(string libraryLabel, int productId, bool isPublic);

        /// <summary>
        /// Delete product from library
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<bool> DeleteProductAsync(int id, int productId);

        /// <summary>
        /// Delete product from library
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteLibraryAsync(int id);

        /// <summary>
        /// Delete product from library
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<bool> DeleteProductAsync(string typeName, int productId);

        /// <summary>
        /// Get libraries id where product is stored
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> GetByProductIdAsync(int productId);

        /// <summary>
        /// Update library
        /// </summary>
        /// <param name="id"></param>
        /// <param name="label"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        Task<bool> UpdateLibraryAsync(int id, string label, bool isPublic);
    }
}
