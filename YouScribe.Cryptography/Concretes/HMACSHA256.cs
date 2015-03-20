using System.Security.Cryptography;
using Youscribe.Rest.Cryptography;

namespace Youscribe.Rest.Concretes
{
    public class YSHMACSHA256 : IHMAC
    {
        public byte[] ComputeHash(byte[] key, byte[] message)
        {
            var hash = new HMACSHA256(key);
            return hash.ComputeHash(message);
        }
    }
}
