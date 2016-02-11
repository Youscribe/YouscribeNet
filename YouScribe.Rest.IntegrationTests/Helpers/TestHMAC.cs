using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Cryptography;

namespace YouScribe.Rest.IntegrationTests.Helpers
{
    class TestHMAC : IHMAC
    {
        public byte[] ComputeHash(byte[] message, byte[] secretKey)
        {
            var encoding = new System.Text.UnicodeEncoding();
            return encoding.GetBytes("FakeHash");
        }
    }
}
