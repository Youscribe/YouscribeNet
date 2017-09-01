using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Products
{
    public class ProductSearchFacetOutputModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public IEnumerable<ProductSearchFacetOutputModel> Childs { get; set; }
        public uint Count { get; set; }
    }
}
