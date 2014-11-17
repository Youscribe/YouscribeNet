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
            return string.Join("&", dico.Where(c => !string.IsNullOrEmpty(c.Value)).Select(c => c.Key + "=" + Uri.EscapeDataString(c.Value)));
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
                if (!(value is IEnumerable) || (value is string))
                    value = new[] { value }; // transform to enumerable
                foreach (var item in (value as IEnumerable).Cast<object>())
                {
                    var strValue = Uri.EscapeDataString(item.ToString());
                    if (!first) builder.Append("&");
                    builder.Append(prop.Name).Append("=").Append(strValue);
                    first = false;
                }
            }
            return builder.ToString();
        }
    }
}
