using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    class AccountDeviceRequest : YouScribeRequest, IAccountDeviceRequest
    {
        public AccountDeviceRequest(IRestClient client, string token)
            : base(client, token)
        {
        }

        public bool AddDevice(DeviceTypeName deviceTypeName, string os, string osVersion, string deviceId)
        {
            var requesst = this.createRequest(ApiUrls.AccountDeviceUrl, Method.PUT);
            requesst.AddBody(new { DeviceTypeName = deviceTypeName.ToString(), Os = os, OsVersion = osVersion, DeviceId = deviceId });

            var response = client.Execute(requesst);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }

        public IEnumerable<DeviceInformation> GetDevices()
        {
            var request = this.createRequest(ApiUrls.AccountDeviceUrl, Method.GET);

            var response = client.Execute<List<DeviceInformation>>(request);

            if (response.Data == null)
                return Enumerable.Empty<DeviceInformation>();
            return response.Data;
        }
    }
}
