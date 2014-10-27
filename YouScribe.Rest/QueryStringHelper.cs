using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest
{
    internal static class QueryStringHelper
    {
        public static string ToQueryString(this IDictionary<string, string> dico)
        {
            return string.Join("&", dico.Select(c => c.Key + "=" + Uri.EscapeDataString(c.Value)));
        }
    }
}
