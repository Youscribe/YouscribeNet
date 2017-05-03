namespace YouScribe.Rest.Models.Products
{
    public class ProductUserPublicKeyModel
    {
        public int ProductId { get; set; }
        public string UserPublicKey { get; set; }
        public string Extension { get; set; }
        public int ExtensionFormatTypeId { get; set; }
    }
}
