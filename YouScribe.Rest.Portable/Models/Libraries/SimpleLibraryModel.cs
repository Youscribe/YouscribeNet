using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Libraries
{
    public class SimpleLibraryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string TypeName { get; set; }

        public int AccountId { get; set; }

        public int ProductPublicCount { get; set; }

        public int ProductPrivateCount { get; set; }

        public LibraryProductModel LastProduct { get; set; }
    }
}
