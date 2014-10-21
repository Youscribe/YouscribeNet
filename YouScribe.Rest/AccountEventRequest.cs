using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using YouScribe.Rest.Models.Accounts;

namespace YouScribe.Rest
{
    class AccountEventRequest : YouScribeRequest, IAccountEventRequest
    {
        public AccountEventRequest(IRestClient client, string authorizeToken)
            : base(client, authorizeToken)
        { }

        public IEnumerable<Models.Accounts.AccountEventModel> ListAllEvents()
        {
            var request = this.createRequest(ApiUrls.AccountEventUrl, Method.GET);

            var response = client.Execute<List<AccountEventModel>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                this.addErrors(response);
                return Enumerable.Empty<Models.Accounts.AccountEventModel>();
            }
            return response.Data;
        }

        public bool SubscribeToEvent(Models.Accounts.AccountEventModel @event)
        {
            var request = this.createRequest(ApiUrls.AccountEventUrl, Method.PUT);
            request.AddBody(@event);

            var response = client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }

        public bool UnSubscribeToEvent(Models.Accounts.AccountEventModel @event)
        {
            var request = this.createRequest(ApiUrls.AccountUnSubscribeEventUrl, Method.DELETE)
                .AddUrlSegment("id", @event.Id.ToString())
                ;

            var response = client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }

        public bool SetEventFrequency(Models.Accounts.NotificationFrequency frequency)
        {
            var request = this.createRequest(ApiUrls.AccountEventFrequencyUrl, Method.PUT)
                .AddUrlSegment("frequency", frequency.ToString())
                ;

            var response = client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }
    }
}
