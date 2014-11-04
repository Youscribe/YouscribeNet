using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Products
{
    public class ProductSearchInputModel
    {
        public ProductSearchInputModel()
        {
            this.take = 10;
        }

        public IEnumerable<int> id { get; set; }

        public int? theme_id { get; set; }

        public int? category_id { get; set; }

        public string quicksearch { get; set; }

        public string author { get; set; }

        public IEnumerable<string> offer_type { get; set; }

        public string title { get; set; }

        public string domain_language { get; set; }

        public bool? is_adult_content { get; set; }

        public int skip { get; set; }

        public int take { get; set; }
    }
}
