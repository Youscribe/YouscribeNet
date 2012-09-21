using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Accounts
{
    public class AccountPublisherModel
    {
        public bool IsProfessional { get; set; }

        public string CorporateName { get; set; }
        public string SiretNumber { get; set; }
        public string VATNumber { get; set; }

        public string Street { get; set; }

        public string Street2 { get; set; }

        public string ZipCode { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string CountryCode { get; set; }

        public Civility Civility { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class AccountPublisherPaypalModel : AccountPublisherModel
    {
        public string PaypalEmail { get; set; }
    }

    public class AccountPublisherTransferModel : AccountPublisherModel
    {
        public string BankName { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
    }
}
