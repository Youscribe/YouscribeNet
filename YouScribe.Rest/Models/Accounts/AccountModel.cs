using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest.Models.Accounts
{
    public class FacebookAccountGetModel : AccountGetModel
    {
        public bool IsNewUser { get; set; }        
    }

    public class AccountGetModel : AccountModel
    {
        public AccountGetModel()
        {
            this.AvatarUrls = Enumerable.Empty<ImageUrlOutput>();
        }

        public IEnumerable<ImageUrlOutput> AvatarUrls { get; set; }
    }

    public class AccountModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

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
        public EmailStatus? EmailStatus { get; set; }

        /// <summary>
        /// The user domain language iso code alpha 2 in lower (ex: "fr", "en", "es")
        /// </summary>
        public string DomainLanguageIsoCode { get; set; }

        public Guid TrackingId { get; set; }

        public string YsAuthToken { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum Civility
    {
        Mr,
        Mrs,
        Miss,
    }

    public enum NotificationFrequency : int
    {
        RealTime,
        ByDay,
        ByWeek,
        Never
    }

    public enum EmailStatus
    {
        Valid,
        Invalid 
    }
}
