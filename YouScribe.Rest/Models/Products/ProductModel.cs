using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Products
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Collection { get; set; }

        public DateTime? PublishDate { get; set; }

        public string EAN13 { get; set; }

        public bool Public { get; set; }

        public bool IsFree { get; set; }

        public decimal? Price { get; set; }

        public List<PeopleModel> People { get; set; }

        public List<string> Languages { get; set; }

        public List<string> Tags { get; set; }

        public int CategoryId { get; set; }

        public int ThemeId { get; set; }

        public bool AllowPasteAndCut { get; set; }

        public bool AllowPrint { get; set; }

        public bool AllowPrintOnDemand { get; set; }

        public bool AllowDownload { get; set; }

        public bool AllowStreaming { get; set; }

        public bool IsAdultContent { get; set; }

        public int? PreviewNbPage { get; set; }
        public int? PreviewPercentPage { get; set; }
        public string PreviewRange { get; set; }

        public Copyright CopyrightInformation { get; set; }

        public int? OwnerId { get; set; }

        public List<string> BrandNames { get; set; }

        public string LicenceName { get; set; }
    }

    public enum Copyright
    {
        IsAuthor,
        IsBacker,
        RightTransfered,
        HasCommercialRight,
        CopyrightFree
    }
}
