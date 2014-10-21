using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    public interface IAccountDeviceRequest : IYouScribeRequest
    {
        /// <summary>
        /// Add device to current account
        /// </summary>
        /// <param name="deviceTypeName">Type of device (phone, tablet, ...)</param>
        /// <param name="os">Os name</param>
        /// <param name="osVersion">Os version</param>
        /// <param name="deviceId">unique identifier for the device</param>
        /// <returns></returns>
        bool AddDevice(DeviceTypeName deviceTypeName, string os, string osVersion, string deviceId);

        /// <summary>
        /// Get user devices
        /// </summary>
        /// <returns>List of user device</returns>
        IEnumerable<DeviceInformation> GetDevices();
    }
}
