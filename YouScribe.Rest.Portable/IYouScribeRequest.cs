using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest
{
    public interface IYouScribeRequest
    {
        /// <summary>
        /// The errors return by the server
        /// </summary>
        IEnumerable<string> Errors { get; }
    }
}
