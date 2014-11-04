using System;
using System.Collections;
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

        public static string ToQueryString(this object obj)
        {
            var properties = obj.GetType().GetProperties();
            var builder = new StringBuilder();
            var first = true;
            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj, null);
                if (value == null) continue;
                string strValue = null;
                if (value is IEnumerable && !(value is string))
                    strValue = string.Join(",", (value as IEnumerable).Cast<object>().Select(c => Uri.EscapeDataString(c.ToString())));
                else
                    strValue = Uri.EscapeDataString(value.ToString());
                if (!first) builder.Append("&");
                builder.Append(prop.Name).Append("=").Append(strValue);
                first = false;
            }
            return builder.ToString();
        }
    }
}
