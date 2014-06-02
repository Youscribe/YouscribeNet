using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAccountUsertTypeRequest : IYouScribeRequest
    {
        /// <summary>
        /// List all the user types
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserTypeModel> ListAllUserTypes();

        /// <summary>
        /// Set the user type
        /// </summary>
        /// <param name="userType">user type</param>
        /// <returns></returns>
        bool SetUserType(UserTypeModel userType);
    }
}
