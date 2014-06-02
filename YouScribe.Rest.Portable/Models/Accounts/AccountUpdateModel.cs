using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models.Accounts
{
    public class AccountUpdateModel
    {
        public string Password { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public Civility? Civility { get; set; }
        public DateTime? BirthDate { get; set; }

        public string CountryCode { get; set; }
        public string BlogUrl { get; set; }
        public string WebSiteUrl { get; set; }
        public string FacebookPage { get; set; }
        public string TwitterPage { get; set; }
        public string City { get; set; }
        public string Biography { get; set; }
        public string PhoneNumber { get; set; }

        public bool EmailIsPublic { get; set; }
    }
}
