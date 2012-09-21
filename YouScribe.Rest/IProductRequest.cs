using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models;
using YouScribe.Rest.Models.Products;

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
        /// <param name="documentId">The document id</param>
        /// <param name="productInformation">All The document information needed</param>
        void UpdateDocument(int documentId, ProductUpdateModel productInformation);

        /// <summary>
        /// Update a document and The files
        /// </summary>
        /// <param name="documentId">The document id</param>
        /// <param name="productInformation">All The document information needed</param>
        /// <param name="files">The documents of The publication. Each document has to have a different format (PDF / EPUB / MOBI). 3 max</param>
        void UpdateDocument(int documentId, ProductUpdateModel productInformation, IEnumerable<FileModel> files);

        /// <summary>
        /// Update a document and The files
        /// </summary>
        /// <param name="documentId">The document id</param>
        /// <param name="productInformation">All The document information needed</param>
        /// <param name="filesUri">The document urls of The publication. Each document has to have a different format (PDF / EPUB / MOBI). 3 max<</param>
        void UpdateDocument(int documentId, ProductUpdateModel productInformation, IEnumerable<Uri> filesUri);

        /// <summary>
        /// Update The document thumbnail from an url
        /// </summary>
        /// <param name="documentId">The document id</param>
        /// <param name="imageUri">The url of The image</param>
        void UpdateDocumentThumbnail(int documentId, Uri imageUri);

        /// <summary>
        /// Update The document thumbnail using The page of The document
        /// </summary>
        /// <param name="documentId">The document id</param>
        /// <param name="page">The page of The document needed for The thumbnail</param>
        void UpdateDocumentThumbnail(int documentId, int page);

        /// <summary>
        /// Update the document thumbnail using an image stream
        /// </summary>
        /// <param name="documentId">The document id</param>
        /// <param name="image">The image stream</param>
        void UpdateDocumentThumbnail(int documentId, FileModel image);
    }
}
