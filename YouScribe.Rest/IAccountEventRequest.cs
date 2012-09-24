using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        IEnumerable<AccountEventModel> ListAllEvents();

        /// <summary>
        /// Subscribe the account to an event
        /// </summary>
        /// <param name="event">The event information</param>
        /// <returns>True if success</returns>
        bool SubscribeToEvent(AccountEventModel @event);
        
        /// <summary>
        /// UnSubscribe the account to an event
        /// </summary>
        /// <param name="event">The event information</param>
        /// <returns>True if success</returns>
        bool UnSubscribeToEvent(AccountEventModel @event);

        /// <summary>
        /// Set the frequency to receive email when event is triggered
        /// </summary>
        /// <param name="frequency">The frequency</param>
        /// <returns>True if success</returns>
        bool SetEventFrequency(NotificationFrequency frequency);
    }
}
