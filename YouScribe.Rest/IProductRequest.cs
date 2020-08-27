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
        /// Get product info
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductGetModel> GetAsync(int id);

        /// <summary>
        /// Get product info (v2 means new image format)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductGetModel> GetAsyncV2(int id);

        /// <summary>
        /// Get products info
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProductGetModel>> GetAsync(IEnumerable<int> ids);

        /// <summary>
        /// Get products info (v2 means new image format)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<ProductGetModel>> GetAsyncV2(IEnumerable<int> ids);

        /// <summary>
        /// Publish a new document
        /// </summary>
        /// <param name="productInformation">All The document information needed</param>
        /// <param name="files">The documents of The publication. Each document has to have a different format (PDF / EPUB / MOBI). 3 max</param>
        /// <returns>The product information with The document id created</returns>
        Task<ProductModel> PublishDocumentAsync(ProductModel productInformation, IEnumerable<FileModel> files);

        /// <summary>
        /// Publish a new document
        /// </summary>
        /// <param name="productInformation">All The document information needed</param>
        /// <param name="filesUri">The document urls of The publication. Each document has to have a different format (PDF / EPUB / MOBI). 3 max</param>
        /// <returns></returns>
        Task<ProductModel> PublishDocumentAsync(ProductModel productInformation, IEnumerable<Uri> filesUri);

        /// <summary>
        /// Update a document
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="productInformation">All The document information needed</param>
        /// <returns>True if success</returns>
        Task<bool> UpdateDocumentAsync(int productId, ProductUpdateModel productInformation);

        /// <summary>
        /// Update a document and The files
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="productInformation">All The document information needed</param>
        /// <param name="files">The documents of The publication. Each document has to have a different format (PDF / EPUB / MOBI). 3 max</param>
        /// <returns>True if success</returns>
        Task<bool> UpdateDocumentAsync(int productId, ProductUpdateModel productInformation, IEnumerable<FileModel> files);

        /// <summary>
        /// Update a document and The files
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="productInformation">All The document information needed</param>
        /// <param name="filesUri">The document urls of The publication. Each document has to have a different format (PDF / EPUB / MOBI). 3 max<</param>
        /// <returns>True if success</returns>
        Task<bool> UpdateDocumentAsync(int productId, ProductUpdateModel productInformation, IEnumerable<Uri> filesUri);

        /// <summary>
        /// Update The document thumbnail from an url
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="imageUri">The url of The image</param>
        /// <returns>True if success</returns>
        Task<bool> UpdateDocumentThumbnailAsync(int productId, Uri imageUri);

        /// <summary>
        /// Update The document thumbnail using The page of The document
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="page">The page of The document needed for The thumbnail</param>
        /// <returns>True if success</returns>
        Task<bool> UpdateDocumentThumbnailAsync(int productId, int page);

        /// <summary>
        /// Update the document thumbnail using an image stream
        /// </summary>
        /// <param name="productId">The product id</param>
        /// <param name="image">The image stream</param>
        /// <returns>True if success</returns>
        Task<bool> UpdateDocumentThumbnailAsync(int productId, FileModel image);


        Task<bool> UpdateMetaAsync(int productId, ProductUpdateModel productInformation);

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
        Task<int> GetRightAsync(int productId);

        /// <summary>
        /// Get products right for current user
        /// </summary>
        /// <param name="productId"The product id></param>
        /// <returns>
        /// Right level :
        /// 0 - Access denies
        /// 100 - View metadata allowed
        /// 110 - Streaming allowed
        /// 120 - Download allowed
        /// </returns>
        Task<IEnumerable<RightModel>> GetRightAsync(IEnumerable<int> productId);

        /// <summary>
        /// Download a specific file for product by extension
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task<Stream> DownloadFileAsync(int productId, string extension);

        /// <summary>
        /// Download a specific file for product by format type id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task<Stream> DownloadFileAsync(int productId, int formatTypeId);

        /// <summary>
        /// Download a specific file for product by extension (version 2)
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task<Stream> DownloadFileV2Async(int productId, string extension);

        /// <summary>
        /// Download a specific file for product by format type id (version 2)
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task<Stream> DownloadFileV2Async(int productId, int formatTypeId);


        /// <summary>
        /// Download a specific file for product by extension
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task DownloadFileToStreamAsync(int productId, string extension, Stream writer, IProgress<DownloadBytesProgress> progressReport);

        /// <summary>
        /// Download a specific file for product by format type id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task DownloadFileToStreamAsync(int productId, int formatTypeId, Stream writer, IProgress<DownloadBytesProgress> progressReport);

        /// <summary>
        /// Download a specific file for product by extension (version 2)
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task DownloadFileToStreamV2Async(int productId, string extension, Stream writer, IProgress<DownloadBytesProgress> progressReport);

        /// <summary>
        /// Download a specific file for product by format type id (version 2)
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task DownloadFileToStreamV2Async(int productId, int formatTypeId, Stream writer, IProgress<DownloadBytesProgress> progressReport);

        /// <summary>
        /// Download a specific file for product by extension
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task<Stream> DownloadExtractAsync(int productId, string extension);

        /// <summary>
        /// Download a specific file for product by format type id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task<Stream> DownloadExtractAsync(int productId, int formatTypeId);

        /// <summary>
        /// Download a specific file for product by extension
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task DownloadExtractToStreamAsync(int productId, string extension, Stream writer, IProgress<DownloadBytesProgress> progressReport);

        /// <summary>
        /// Download a specific file for product by format type id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        Task DownloadExtractToStreamAsync(int productId, int formatTypeId, Stream writer, IProgress<DownloadBytesProgress> progressReport);

        /// <summary>
        /// Retunr products urls
        /// </summary>
        /// <param name="ids">products id</param>
        /// <returns></returns>
        Task<IEnumerable<ProductUrlsModel>> GetProductUrlsAsync(IEnumerable<int> ids);

        /// <summary>
        /// Get a direct download link for a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ProductDownloadLinkOutputModel> GetProductDownloadLinkAsync(int productId);

        /// <summary>
        /// Return encrypted key for product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="extension"></param>
        /// <param name="userPublicKey"></param>
        /// <returns></returns>
        Task<string> PostEncryptedKeyByExtension(int productId, string extension, string userPublicKey);

        /// <summary>
        /// Return encrypted key for product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="formatTypeId"></param>
        /// <param name="userPublicKey"></param>
        /// <returns></returns>
        Task<string> PostEncryptedKeyByFormatTypeId(int productId, int formatTypeId, string userPublicKey);
    }
}
