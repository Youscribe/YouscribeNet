using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest.Models.Libraries
{
    public class LibraryProductModel
    {
        public bool IsPublic { get; set; }

        public DateTime CreationDate { get; set; }

        public ProductGetModel Product { get; set; }
    }
}
