using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Accounts
{
    public class AccountPropertiesInputModel
    {
        public IEnumerable<AccountPropertyInputModel> Properties { get; set; }
    }
    public class AccountPropertyInputModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
