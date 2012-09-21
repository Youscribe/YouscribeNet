using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest
{
    public interface IYouScribeRequest
    {
        IEnumerable<string> Errors { get; }
    }
}
