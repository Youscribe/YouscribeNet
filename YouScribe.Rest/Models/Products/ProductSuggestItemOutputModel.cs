using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Products
{
    public class ProductSuggestItemOutputModel
    {
        public ProductSuggestItemOutputModel()
        {
            this.ThumbnailUrls = Enumerable.Empty<ImageUrlOutput>();
            this.OfferTypes = Enumerable.Empty<string>();
            this.Authors = Enumerable.Empty<string>();
            this.PublicFormatExtensions = Enumerable.Empty<string>();
            this.LanguagesIsoCode = Enumerable.Empty<string>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public string OwnerUserName { get; set; }

        public int NbPages { get; set; }

        public IEnumerable<string> OfferTypes { get; set; }

        public IEnumerable<string> PublicFormatExtensions { get; set; }

        public IEnumerable<ImageUrlOutput> ThumbnailUrls { get; set; }

        public ThemeModel Theme { get; set; }

        public CategoryModel Category { get; set; }

        public int StateId { get; set; }

        public IEnumerable<string> LanguagesIsoCode { get; set; }

        public int? ModelId { get; set; }

        public TimeSpan EstimatedReadTime { get; set; }

    }
}
