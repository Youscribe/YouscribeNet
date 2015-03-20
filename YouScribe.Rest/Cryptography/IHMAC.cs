namespace Youscribe.Rest.Cryptography
{
    public interface IHMAC
    {
        byte[] ComputeHash(byte[] message, byte[] secretKey);
    }
}
