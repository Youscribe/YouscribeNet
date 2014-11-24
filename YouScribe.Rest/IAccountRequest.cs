using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    public enum DeviceTypeName
    {
        Phone,
        Tablet,
        TV,
        Computer
    }

    public interface IAccountRequest : IYouScribeRequest
    {
        /// <summary>
        /// Get current account async
        /// </summary>
        /// <returns>The current account</returns>
        Task<Models.Accounts.AccountModel> GetCurrentAccountAsync();

        /// <summary>
        /// Register a new account
        /// The UserName, Email and Password fields are required
        /// </summary>
        /// <param name="account">The account information</param>
        /// <returns>Returns the account created</returns>
        Task<AccountModel> CreateAsync(AccountModel account);

        /// <summary>
        /// Update your account
        /// </summary>
        /// <param name="account">The account information</param>
        /// <returns>True if success</returns>
        Task<bool> UpdateAsync(AccountModel account);

        /// <summary>
        /// Set the spoken languages of the account
        /// </summary>
        /// <param name="languages">A two or three letter language iso code</param>
        /// <returns>True if success</returns>
        Task<bool> SetSpokenLanguagesAsync(IEnumerable<string> languages);

        /// <summary>
        /// Update account picture
        /// </summary>
        /// <param name="uri">The uri of the photo</param>
        /// <returns>True if success</returns>
        Task<bool> UploadPictureAsync(Uri uri);

        /// <summary>
        /// Update account picture
        /// </summary>
        /// <param name="image">The image information of the photo. The format accepetd are gif / jpeg / png / bmp </param>
        /// <returns>True if success</returns>
        Task<bool> UploadPictureAsync(FileModel image);

        /// <summary>
        /// Delete the account photo
        /// </summary>
        /// <returns></returns>
        Task<bool> DeletePictureAsync();
    }
}
