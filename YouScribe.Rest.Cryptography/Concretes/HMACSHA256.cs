using System.Security.Cryptography;

namespace YouScribe.Rest.Cryptography.Concretes
{
    public class YSHMACSHA256 : IHMAC
    {
        public byte[] ComputeHash(byte[] message, byte[] secretKey)
        {
            var hash = new HMACSHA256(secretKey);
            return hash.ComputeHash(message);
        }
    }
}
