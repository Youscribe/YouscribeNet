using System.Security.Cryptography;

namespace Youscribe.Rest.Cryptography.Concretes
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
