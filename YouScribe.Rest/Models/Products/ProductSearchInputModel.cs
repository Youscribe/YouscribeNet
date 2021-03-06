﻿using System;
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
            this.offer_type = Enumerable.Empty<string>();
            this.sort = Enumerable.Empty<string>();
            this.id = Enumerable.Empty<int>();
            this.requested_facet = Enumerable.Empty<string>();
            this.excluded_theme_id = Enumerable.Empty<int>();
        }

        public IEnumerable<int> id { get; set; }

        public int? theme_id { get; set; }

        public int? category_id { get; set; }

        public string quicksearch { get; set; }

        public string author { get; set; }

        public IEnumerable<string> offer_type { get; set; }

        public IEnumerable<string> excluded_offer_type { get; set; }

        public IEnumerable<string> public_format_extension { get; set; }

        public string title { get; set; }

        public string domain_language { get; set; }

        public bool? is_adult_content { get; set; }

        public int skip { get; set; }

        public int take { get; set; }

        public IEnumerable<string> sort { get; set; }

        public int? language_id { get; set; }

        public int? price_group { get; set; }

        public int? access_type { get; set; }

        public IEnumerable<int> excluded_theme_id { get; set; }
        public IEnumerable<int> excluded_category_id { get; set; }
        public IEnumerable<string> requested_facet { get; set; }

        public int? nb_pages_min { get; set; }
        public int? nb_pages_max { get; set; }

        public IEnumerable<int> sensibility_id { get; set; }
        public IEnumerable<int> rubric_id { get; set; }
        public IEnumerable<int> in_owner_id { get; set; }
        public int? tag_id { get; set; }
        public IEnumerable<int> in_tags_id { get; set; }
        public IEnumerable<int> tags_id { get; set; }

        public int? owner_Id { get; set; }
        public SearchableFlag? is_searchable { get; set; }
        public VisibilityFlag? is_public { get; set; }
        public IEnumerable<int> state_id { get; set; }
        public IList<int> excluded_language_id { get; set; }
        public bool? is_free { get; set; }
        public IList<int> excluded_product_id { get; set; }        
    }
    public enum VisibilityFlag
    {
        Public,
        Private,
        All
    }
    public enum SearchableFlag
    {
        Searchable,
        NotSearchable,
        All
    }
}
