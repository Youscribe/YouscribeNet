using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Products
{
    public class PropertyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public PropertyTypeModel Type { get; set; }
    }
}
