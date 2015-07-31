using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Products
{
    public class ProductSearchItemOutputModel
    {
        public ProductSearchItemOutputModel()
        {
            this.ThumbnailUrls = Enumerable.Empty<ImageUrlOutput>();
            this.OfferTypes = Enumerable.Empty<string>();
            this.Authors = Enumerable.Empty<string>();
            this.PublicFormatExtensions = Enumerable.Empty<string>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public string OwnerUserName { get; set; }

        public int NbPages { get; set; }

        public int NbReads { get; set; }

        public double AvgScore { get; set; }

        public int NbVotes { get; set; }

        public double PertinenceWeight { get; set; }

        public IEnumerable<string> OfferTypes { get; set; }

        public IEnumerable<string> PublicFormatExtensions { get; set; }

        public IEnumerable<string> ExtractPublicFormatExtensions { get; set; }

        public IEnumerable<ImageUrlOutput> ThumbnailUrls { get; set; }

        public ThemeModel Theme { get; set; }

        public int StateId { get; set; }
        
    }

    public class ProductSearchOutputModel
    {
        public int TotalResults { get; set; }

        public IEnumerable<ProductSearchItemOutputModel> Products { get; set; }
    }
}
