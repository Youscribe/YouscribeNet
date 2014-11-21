using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Task<IEnumerable<UserTypeModel>> ListAllUserTypesAsync();

        /// <summary>
        /// Set the user type
        /// </summary>
        /// <param name="userType">user type</param>
        /// <returns></returns>
        Task<bool> SetUserTypeAsync(UserTypeModel userType);
    }
}
