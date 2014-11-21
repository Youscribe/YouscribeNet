using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    /// <summary>
    /// Manage the account subscriptions
    /// </summary>
    public interface IAccountEventRequest : IYouScribeRequest
    {        
        /// <summary>
        /// List all the events defined on YouScribe
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<AccountEventModel>> ListAllEventsAsync();

        /// <summary>
        /// Subscribe the account to an event
        /// </summary>
        /// <param name="event">The event information</param>
        /// <returns>True if success</returns>
        Task<bool> SubscribeToEventAsync(AccountEventModel @event);
        
        /// <summary>
        /// UnSubscribe the account to an event
        /// </summary>
        /// <param name="event">The event information</param>
        /// <returns>True if success</returns>
        Task<bool> UnSubscribeToEventAsync(AccountEventModel @event);

        /// <summary>
        /// Set the frequency to receive email when event is triggered
        /// </summary>
        /// <param name="frequency">The frequency</param>
        /// <returns>True if success</returns>
        Task<bool> SetEventFrequencyAsync(NotificationFrequency frequency);
    }
}
