using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    class AccountDeviceRequest : YouScribeRequest, IAccountDeviceRequest
    {
        public AccountDeviceRequest(Func<HttpClient> clientFactory, string token)
            : base(clientFactory, token)
        {
        }

        public async Task<bool> AddDevice(DeviceTypeName deviceTypeName, string os, string osVersion, string deviceId)
        {
            using (var client = this.CreateClient())
            {
                var content = this.GetContent(new DeviceInputModel() { DeviceTypeName = deviceTypeName.ToString(), Os = os, OsVersion = osVersion, DeviceId = deviceId });
                var response = await client.PutAsync(ApiUrls.AccountDeviceUrl, content);
                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }

        public async Task<IEnumerable<DeviceInformation>> GetDevices()
        {
            using (var client = this.CreateClient())
            {
                var response = await client.GetAsync(ApiUrls.AccountDeviceUrl);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await this.AddErrorsAsync(response);
                    return Enumerable.Empty<DeviceInformation>();
                }
                return await this.GetObjectAsync<IEnumerable<DeviceInformation>>(response.Content);
            }
        }
    }
}
