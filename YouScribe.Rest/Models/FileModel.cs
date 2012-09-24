using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models
{
    public class FileModel
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Stream Content { get; set; }

        public bool IsValid
        {
            get
            {
                return String.IsNullOrEmpty(this.FileName) == false &&
                    String.IsNullOrEmpty(this.ContentType) == false &&
                    this.Content != null;
            }
        }
    }

    public class FileUrlModel
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Uri Uri { get; set; }

        public bool IsValid
        {
            get
            {
                return String.IsNullOrEmpty(this.FileName) == false &&
                    String.IsNullOrEmpty(this.ContentType) == false &&
                    this.Uri != null && !(this.Uri.IsFile || this.Uri.IsLoopback || this.Uri.IsUnc);
            }
        }
    }
}
