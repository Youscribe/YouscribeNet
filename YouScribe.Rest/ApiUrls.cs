using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest
{
    public static class ApiUrls
    {
        public const string BaseUrl = "https://services.youscribe.com";

        public const string AuthorizeUrl = "api/authorize";

        public const string AccountUrl = "api/v1/accounts";
        public const string AccountEventUrl = "api/v1/accounts/events";
        public const string AccountSubscribeEventUrl = "api/v1/accounts/events/{name}";
        public const string AccountDeviceUrl = "api/v1/accounts/devices";
        public const string AccountUnSubscribeEventUrl = "api/v1/accounts/events?id={id}";
        public const string AccountEventFrequencyUrl = "api/v1/accounts/events?frequency={frequency}";
        public const string AccountPaypalPublisherUrl = "api/v1/accounts/paypalpublisher";
        public const string AccountTransferPublisherUrl = "api/v1/accounts/transferpublisher";
        public const string AccountLanguagesUrl = "api/v1/accounts/languages";
        public const string AccountUserTypesUrl = "api/v1/accounts/usertypes";

        public const string PostEncryptedKeyByExtensionUrlV2 = "api/v2/products/files/key/extension";
        public const string PostEncryptedKeyByFormatTypeIdUrlV2 = "api/v2/products/files/key/formatTypeId";

        public const string PictureUrl = "api/v1/pictures";

        public const string ProductUrl = "api/v1/products";
        public const string ProductUrlByIds = "api/v1/products/byids";
        public const string ProductUrlByIdsV2 = "api/v2/products/byids";
        public const string ProductGetUrlsByIds = "api/v1/products/urls/byids";
        public const string ProductSearchUrl = "api/v1/products/search";
        public const string ProductSearchUrlV2 = "api/v2/products/search";
        public const string ProductUpdateUrl = "api/v1/products/{id}";
        public const string ProductGetUrl = "api/v1/products/{id}";
        public const string ProductGetUrlV2 = "api/v2/products/{id}";
        public const string ProductGetDownloadLinkUrl = "api/v1/product/downloadlink/{id}";
        public const string ProductEndUploadUrl = "api/v1/products/endupload?id={id}";
        public const string ProductEndUpdateUrl = "api/v1/products/endupdate?id={id}";

        public const string ProductUpdateMetaUrl = "api/v1/products/update-meta/{id}";

        [Obsolete]
        public const string ProductDownloadByExtensionUrl = "api/v1/products/{id}/files/{extension}";
        [Obsolete]
        public const string ProductDownloadByFormatTypeIdUrl = "api/v1/products/{id}/files/{formatTypeId}";

        public const string ProductDownloadByExtensionUrlV2 = "api/v2/products/{id}/files/{extension}";
        public const string ProductDownloadByFormatTypeIdUrlV2 = "api/v2/products/{id}/files/{formatTypeId}";

        public const string ProductDownloadExtractByExtensionUrl = "api/v1/products/{id}/extracts/{extension}";
        public const string ProductDownloadExtractByFormatTypeIdUrl = "api/v1/products/{id}/extracts/{formatTypeId}";

        public const string ProductRightUrl = "api/v1/productrights/{id}";
        public const string ProductRightUrlByIds = "api/v1/productrights";

        public const string LibraryUrl = "api/v1/libraries";
        public const string LibraryGetUrl = "api/v1/libraries/{id}";
        public const string LibraryGetByTypeNameUrl = "api/v1/libraries/{typeName}";
        public const string LibraryDeleteUrl = LibraryGetUrl;
        public const string LibraryUpdateUrl = LibraryGetUrl;
        public const string LibraryAddProductUrl = "api/v1/libraries/{id}/product/{productId}";
        public const string LibraryDeleteProductUrl = LibraryAddProductUrl;
        public const string LibraryAddInCustomProductUrl = "api/v1/libraries/custom/product/{productId}";
        public const string LibraryAddByTypeNameProductUrl = "api/v1/libraries/{typeName}/product/{productId}";
        public const string LibraryDeleteByTypeNameProductUrl = LibraryAddByTypeNameProductUrl;
        public const string LibraryGetByProductIdUrl = "api/v1/libraries/product/{productId}";

        public const string UploadUrl = "api/v1/upload/{id}";
        public const string UploadFileUrl = "api/v1/upload/{id}?url={url}";

        public const string ThumbnailDataUrl = "api/v1/thumbnail/{id}";
        public const string ThumbnailLinkUrl = "api/v1/thumbnail/{id}?url={url}";
        public const string ThumbnailPageUrl = "api/v1/thumbnail/{id}?page={page}";

        public const string EmbedUrl = "api/v1/embed/{id}";
        public const string PrivateEmbedUrl = "api/v1/embed/private";

        public const string ThemeUrl = "api/themes";
        public const string PropertyUrl = "api/property?type={type}";

        public const string AuthorizeTokenHeaderName = "YS-AUTH";
        public const string HMACAuthenticateRandomKeyHeader = "YS-AUTH-RANDOM-KEY";
        public const string HMACScheme = "YSWS";
    }
}
