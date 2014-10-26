using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    /// <summary>
    /// Updaate the publisher information of the current account
    /// </summary>
    public interface IAccountPublisherRequest : IYouScribeRequest
    {
        /// <summary>
        /// Set the publisher information as Paypal
        /// </summary>
        /// <param name="paypalPublisher">
        /// Paypal publisher information, the PaypalEmail field is required
        /// If the publisher is professional, the CorporateName, SiretNumber and VATNumber fields are required
        /// </param>
        /// <returns></returns>
        Task<bool> SetAsPaypalPublisherAsync(AccountPublisherPaypalModel paypalPublisher);

        /// <summary>
        /// Set the publisher information as Transfer
        /// </summary>
        /// <param name="transferPublisher">
        /// Transfer publisher information, the BankName, IBAN and BIC fields are required
        /// If the publisher is professional, the CorporateName, SiretNumber and VATNumber fields are required
        /// </param>
        /// <returns></returns>
        Task<bool> SetAsTransferPublisherAsync(AccountPublisherTransferModel transferPublisher);
    }
}
