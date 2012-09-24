using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest
{
    static class ApiUrls
    {
        public const string BaseUrl = "http://services.youscribe.com";

        public const string AuthorizeUrl = "api/authorize";

        public const string AccountUrl = "api/v1/accounts";
        public const string AccountEventUrl = "api/v1/accounts/events";
        public const string AccountSubscribeEventUrl = "api/v1/accounts/events?id={id}";
        public const string AccountEventFrequencyUrl = "api/v1/accounts/events?frequency={frequency}";
        public const string AccountPaypalPublisherUrl = "api/v1/accounts/paypalpublisher";
        public const string AccountTransferPublisherUrl = "api/v1/accounts/transferpublisher";
        public const string AccountLanguagesUrl = "api/v1/accounts/languages";
        public const string AccountUserTypesUrl = "api/v1/accounts/usertypes";

        public const string PictureUrl = "api/v1/pictures";
        public const string PictureUpdateUrl = "api/v1/pictures?url={url}";

        public const string ProductUrl = "api/v1/products";
        public const string ProductUpdateUrl = "api/v1/products/{id}";
        public const string ProductEndUploadUrl = "api/v1/products/endupload?id={id}";
        public const string ProductEndUpdateUrl = "api/v1/products/endupdate?id={id}";

        public const string UploadUrl = "api/v1/upload/{id}";
        public const string UploadFileUrl = "api/v1/upload/{id}?url={url}";

        public const string ThumbnailDataUrl = "api/v1/thumbnail/{id}";
        public const string ThumbnailLinkUrl = "api/v1/thumbnail/{id}?url={url}";
        public const string ThumbnailPageUrl = "api/v1/thumbnail/{id}?page={page}";


        public const string AuthorizeTokenHeaderName = "YS_AUTH";

    }
}
