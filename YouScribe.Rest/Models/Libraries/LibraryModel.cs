using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Libraries
{
    public class LibraryModel: SimpleLibraryModel
    {
        public List<LibraryProductModel> Products { get; set; }
    }
}
