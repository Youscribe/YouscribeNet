using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Libraries;

namespace YouScribe.Rest
{
    public interface ILibraryRequest : IYouScribeRequest
    {
        /// <summary>
        /// List all libraries for current user
        /// </summary>
        /// <returns>List of libraries</returns>
        IEnumerable<SimpleLibraryModel> Get();

        /// <summary>
        /// Get library by id
        /// </summary>
        /// <param name="id">library id</param>
        /// <returns>Library</returns>
        LibraryModel Get(int id);

        /// <summary>
        /// Get library by typeName
        /// </summary>
        /// <param name="typeName">library type name</param>
        /// <returns>Library</returns>
        LibraryModel Get(string typeName);

        /// <summary>
        /// Add product to library
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool AddProduct(int id, int productId);

        /// <summary>
        /// Add product to library
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool AddProduct(string typeName, int productId);


        /// <summary>
        /// Delete product from library
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool DeleteProduct(int id, int productId);

        /// <summary>
        /// Delete product from library
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool DeleteProduct(string typeName, int productId);

        /// <summary>
        /// Get libraries id where product is stored
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IEnumerable<int> GetByProductId(int productId);
    }
}
