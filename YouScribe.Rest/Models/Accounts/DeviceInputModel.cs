using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Accounts
{
    public class DeviceInputModel
    {
        public string Os { get; set; }

        public string OsVersion { get; set; }

        public string DeviceTypeName { get; set; }

        public string DeviceId { get; set; }
    }
}
