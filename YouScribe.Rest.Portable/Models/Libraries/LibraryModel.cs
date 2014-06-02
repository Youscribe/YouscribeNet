using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Libraries
{
    public class LibraryModel: SimpleLibraryModel
    {
        public IEnumerable<LibraryProductModel> Products { get; set; }
    }
}
