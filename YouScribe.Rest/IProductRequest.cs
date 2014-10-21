using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models;
using YouScribe.Rest.Models.Products;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    public interface IProductRequest : IYouScribeRequest
    {
        /// <summary>
        /// Publish a new document
        /// </summary>
        /// <param name="productInformation">All The document information needed</param>
        /// <param name="files">The documents of The publication. Each document has to have a different format (PDF / EPUB / MOBI). 3 max</param>
        /// <returns>The product information with The document id created</returns>
        ProductModel PublishDocument(ProductModel productInformation, IEnumerable<FileModel> files);

        /// <summary>
        /// Publish a new document
        /// </summary>
        /// <param name="productInformation">All The document information needed</param>
        /// <param name="filesUri">The document urls of The publication. Each document has to have a different format (PDF / EPUB / MOBI). 3 max</param>
        /// <returns></returns>
        ProductModel PublishDocument(ProductModel productInformation, IEnumerable<Uri> filesUri);

        /// <summary>
        /// Update a document
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="productInformation">All The document information needed</param>
        /// <returns>True if success</returns>
        bool UpdateDocument(int productId, ProductUpdateModel productInformation);

        /// <summary>
        /// Update a document and The files
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="productInformation">All The document information needed</param>
        /// <param name="files">The documents of The publication. Each document has to have a different format (PDF / EPUB / MOBI). 3 max</param>
        /// <returns>True if success</returns>
        bool UpdateDocument(int productId, ProductUpdateModel productInformation, IEnumerable<FileModel> files);

        /// <summary>
        /// Update a document and The files
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="productInformation">All The document information needed</param>
        /// <param name="filesUri">The document urls of The publication. Each document has to have a different format (PDF / EPUB / MOBI). 3 max<</param>
        /// <returns>True if success</returns>
        bool UpdateDocument(int productId, ProductUpdateModel productInformation, IEnumerable<Uri> filesUri);

        /// <summary>
        /// Update The document thumbnail from an url
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="imageUri">The url of The image</param>
        /// <returns>True if success</returns>
        bool UpdateDocumentThumbnail(int productId, Uri imageUri);

        /// <summary>
        /// Update The document thumbnail using The page of The document
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="page">The page of The document needed for The thumbnail</param>
        /// <returns>True if success</returns>
        bool UpdateDocumentThumbnail(int productId, int page);

        /// <summary>
        /// Update the document thumbnail using an image stream
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="image">The image stream</param>
        /// <returns>True if success</returns>
        bool UpdateDocumentThumbnail(int productId, FileModel image);

        /// <summary>
        /// Get product right for current user
        /// </summary>
        /// <param name="productId"The product id></param>
        /// <returns>
        /// Right level :
        /// 0 - Access denies
        /// 100 - View metadata allowed
        /// 110 - Streaming allowed
        /// 120 - Download allowed
        /// </returns>
        int GetRight(int productId);

        /// <summary>
        /// Download a specific file for product by extension
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Stream DownloadFile(int productId, string extension);

        /// <summary>
        /// Download a specific file for product by format type id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Stream DownloadFile(int productId, int formatTypeId);

#if __ANDROID__
        /// <summary>
        /// Download a specific file for product by extension
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task DownloadFileToPathAsync(int productId, string extension, string path, IProgress<DownloadBytesProgress> progressReport);

        /// <summary>
        /// Download a specific file for product by format type id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task DownloadFileToPathAsync(int productId, int formatTypeId, string path, IProgress<DownloadBytesProgress> progressReport);
#else
        /// <summary>
        /// Download a specific file for product by extension
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        void DownloadFileToPath(int productId, string extension, string path);

        /// <summary>
        /// Download a specific file for product by format type id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        void DownloadFileToPath(int productId, int formatTypeId, string path);
#endif
    }
}
