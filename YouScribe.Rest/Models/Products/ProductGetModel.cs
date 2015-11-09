using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Products
{
    public class ProductGetDocumentFormat
    {
        public int Id { get; set; }

        public string MimeType { get; set; }

        public string Extension { get; set; }

        public int FormatTypeId { get; set; }

        public string EAN13 { get; set; }

        public long Size { get; set; }
    }

    public class ProductGetDocument
    {
        public int Id { get; set; }

        public string DocumentProtectionTypeName { get; set; }

        public int? NbPages { get; set; }

        public int PageWidthPoints { get; set; }

        public int PageHeightPoints { get; set; }

        public List<ProductGetDocumentFormat> Formats { get; set; }
    }

    public class ProductGetTag
    {
        public int Id { get; set; }

        public string Value { get; set; }
    }

    public class ProductGetThumbnailUrl
    {
        public int? Width { get; set; }

        public int? Height { get; set; }

        public string Url { get; set; }
    }

    public class ProductGetDistributionInfo
    {
        public bool AllowComment { get; set; }

        public bool IsAdultContent { get; set; }

        public int? PreviewNbPage { get; set; }

        public int? PreviewPercentPage { get; set; }

        public string PreviewPage { get; set; }

        public string DisplayMode { get; set; }
    }

    public class ProductGetOfferModel
    {
        /// <summary>
        /// Offer identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// offer type name
        ///  - Free
        ///  - Buy
        ///  - Subscription
        ///  - PrintOnDemand
        /// </summary>
        public string OfferTypeName { get; set; }

        /// <summary>
        /// Offer allow streaming of product
        /// </summary>
        public bool AllowStreaming { get; set; }

        /// <summary>
        /// Offer allow download of product
        /// </summary>
        public bool AllowDownload { get; set; }

        /// <summary>
        /// Product Price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Currency of the price
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Does price include taxes
        /// </summary>
        public bool PriceTaxIncluded { get; set; }

        /// <summary>
        /// Product TaxRate
        /// </summary>
        public decimal TaxRate { get; set; }

        /// <summary>
        /// Max number of device that can be used
        /// </summary>
        public int? MaxDeviceUse { get; set; }

        /// <summary>
        /// Max number of day the file can be used
        /// </summary>
        public int? MaxDayUse { get; set; }

        /// <summary>
        /// Max number of time the file can be used
        /// </summary>
        public int? MaxTimeUse { get; set; }
    }

    public class ProductGetModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public int? OwnerId { get; set; }

        public string OwnerUserName { get; set; }

        public string OwnerDisplayableUserName

        public int? ThemeId { get; set; }

        public int? CategoryId { get; set; }

        /// <summary>
        /// Product state
        /// </summary>
        public string State { get; set; }

        public string EAN13 { get; set; }

        /// <summary>
        /// Default access right
        /// </summary>
        public int DefaultRight { get; set; }

        public List<string> LanguagesIsoCode { get; set; }

        public List<ProductGetTag> Tags { get; set; }

        public List<ProductGetThumbnailUrl> ThumbnailUrls { get; set; }

        public List<ProductGetOfferModel> Offers { get; set; }

        public ProductGetDistributionInfo DistributionInfo { get; set; }

        /// <summary>
        /// Publish date (might be official publish date)
        /// </summary>
        DateTime? PublishDate { get; set; }

        /// <summary>
        /// Creation date (first insertion in our system)
        /// </summary>
        DateTime CreationDate { get; set; }

        /// <summary>
        /// Online Date (when the publication is online)
        /// </summary>
        DateTime? OnlineDate { get; set; }

        public ProductGetDocument Document { get; set; }

        public ProductGetDocument ExtractDocument { get; set; }

        public TimeSpan EstimatedReadTime { get; set; }
    }
}
