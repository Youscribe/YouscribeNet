using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Accounts;

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
        Task<bool> AuthorizeAsync(string userNameOrEmail, string password, int? validityInHours = null);

        /// <summary>
        /// Set token to use method that need authentification
        /// </summary>
        /// <param name="token"></param>
        void SetToken(string token);

        /// <summary>
        /// Get current token
        /// </summary>
        /// <returns></returns>
        string GetToken();

        /// <summary>
        /// Set user agent used for every requests
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="version"></param>
        void SetUserAgent(string productName, string version);

        /// <summary>
        /// Create product request to manage yours publications
        /// </summary>
        /// <returns></returns>
        IProductRequest CreateProductRequest();

        /// <summary>
        /// Create account request to manage and create account
        /// </summary>
        /// <returns></returns>
        IAccountRequest CreateAccountRequest();

        /// <summary>
        /// Create account event request to manage the subscription and unsubscription to an event
        /// </summary>
        /// <returns></returns>
        IAccountEventRequest CreateAccountEventRequest();

        /// <summary>
        /// Create account publisher request to set the current account as Paypal or transfer publisher
        /// </summary>
        /// <returns></returns>
        IAccountPublisherRequest CreateAccountPublisherRequest();

        /// <summary>
        /// Create account user type request to change the profile type of the accountunsubscription
        /// </summary>
        /// <returns></returns>
        IAccountUsertTypeRequest CreateAccountUserTypeRequest();

        /// <summary>
        /// Create embed request to get the embed code for a product
        /// </summary>
        IEmbedRequest CreateEmbedRequest();

        /// Create library request
        /// </summary>
        /// <returns></returns>
        ILibraryRequest CreateLibraryRequest();

        /// <summary>
        /// Create product comment request
        /// </summary>
        /// <returns></returns>
        IProductCommentRequest CreateProductCommentRequest();

        /// <summary>
        /// Create product search request
        /// </summary>
        /// <returns></returns>
        IProductSearchRequest CreateProductSearchRequest();

        /// <summary>
        /// Create product suggest request
        /// </summary>
        /// <returns></returns>
        IProductSuggestRequest CreateProductSuggestRequest();
    }
}
