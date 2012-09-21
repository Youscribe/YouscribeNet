using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest
{
    public interface IYouScribeClient
    {
        /// <summary>
        /// Authorize an user using its username or email and its clear password
        /// </summary>
        /// <param name="userNameOrEmail"></param>
        /// <param name="password"></param>
        /// <returns>True if authorized</returns>
        bool Authorize(string userNameOrEmail, string password);

        IProductRequest CreateProductRequest();
    }
}
