using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    public interface IAccountUtilRequest : IYouScribeRequest
    {
        Task<string> GeneratePasswordAsync(int minLength, int maxLength);

        Task<string> GetUserNameFromEmailAsync(string email);

        Task<bool> ForgotPasswordAsync(string userNameOrEmail);

        Task<bool> ChangeEmailAsync(Models.Accounts.AccountModel account);
    }
}
