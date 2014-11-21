using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace YouScribe.Rest.IntegrationTests.Helpers
{
    public static class ResquestHelpers
    {
        public static void Write(this Stream outputStream, string content)
        {
            var bytes = Encoding.Default.GetBytes(content);
            outputStream.Write(bytes, 0, bytes.Length);
        }

        public static string GetResponseAsString(this HttpListenerResponse response)
        {
            string content = null;
            using (var reader = new StreamReader(response.OutputStream))
            {
                content = reader.ReadToEnd();
            }
            return content;
        }

        public static string GetRequestAsString(this HttpListenerRequest request)
        {
            string content = null;
            using (var reader = new StreamReader(request.InputStream))
            {
                content = reader.ReadToEnd();
            }
            return content;
        }
    }
}
