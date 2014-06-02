using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Helpers
{
    public static class UriHelpers
    {
        public static bool IsValid(this Uri uri)
        {
            return !uri.IsFile && !uri.IsLoopback && !uri.IsUnc;
        }
    }
}
