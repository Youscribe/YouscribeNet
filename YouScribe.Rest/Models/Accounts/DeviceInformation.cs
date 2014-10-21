using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Accounts
{
    public class DeviceInformation
    {
        public int Id { get; set; }

        public string DeviceTypeName { get; set; }

        public string Os { get; set; }

        public string OsVersion { get; set; }

        public string DeviceId { get; set; }

        public DateTime LastSeen { get; set; }
    }
}
