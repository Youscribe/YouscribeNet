using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Products
{
    public enum ImageType
    {
        Default,
        S,
        M,
        L,
        S_WebP,
        M_WebP,
        L_WebP
    }

    public class ImageUrlOutput
    {
        public string Url { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public ImageType Type { get; set; }
    }

    public class ProductCommentWriterOutput
    {
        public int? Id { get; set; }
        public string UserName { get; set; }
        public bool Enable { get; set; }
        public IEnumerable<ImageUrlOutput> AvatarUrls { get; set; }
        public string DomainLanguage { get; set; }
    }

    public class ProductCommentsOutput
    {
        public int Count { get; set; }

        public IEnumerable<ProductCommentOutput> Comments { get; set; }
    }

    public class ProductCommentOutput
    {
        public Guid Id { get; set; }
        public int ProductId { get; set; }
        public Guid? ParentId { get; set; }
        public string Message { get; set; }
        public DateTime PostDate { get; set; }

        public ProductCommentWriterOutput Writer { get; set; }

        public IEnumerable<ProductCommentOutput> Replies { get; set; }

        public int NbReplies { get; set; }
    }
}
