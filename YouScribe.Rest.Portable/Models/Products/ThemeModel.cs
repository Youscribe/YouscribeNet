using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Products
{
    public class ThemeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public ThemeModel Parent { get; set; }
    }
}
