using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Products
{
    public class EmbedGenerateModel
    {
        /// <summary>
        /// The height of the embed 
        /// </summary>
        public int? Width { get; set; }
        /// <summary>
        /// The width of the embed
        /// </summary>
        public int? Height { get; set; }
        /// <summary>
        /// The start page of the document to start with
        /// </summary>
        public int? StartPage { get; set; }
        /// <summary>
        /// The display mode
        /// </summary>
        public EmbedDisplayMode? DisplayMode { get; set; }
    }
    
    public class PrivateEmbedGenerateModel : EmbedGenerateModel
    {
        /// <summary>
        /// An access period for a private embed
        /// Foramt by days (xd) month (xM) year (xy) seconde (xs) minutes (xm) hour (xh)
        /// Exemple: 1d => one day
        /// </summary>
        public string AccessPeriod { get; set; }
    }

    public class EmbedResponse
    {
        public string Content { get; set; }
    }

    public enum EmbedDisplayMode
    {
        /// <summary>
        /// Vertical scoll display mode
        /// </summary>
        Embed,
        /// <summary>
        /// Double page display mode
        /// </summary>
        Double
    }
}
