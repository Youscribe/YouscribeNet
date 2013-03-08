using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}
