using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Accounts;
using System.Net.Http;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    class AccountEventRequest : YouScribeRequest, IAccountEventRequest
    {
        public AccountEventRequest(Func<HttpClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

        public async Task<IEnumerable<Models.Accounts.AccountEventModel>> ListAllEventsAsync()
        {
            var request = this.createRequest(ApiUrls.AccountEventUrl, Method.GET);

            var response = client.Execute<List<AccountEventModel>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await this.AddErrorsAsync(response);
                return Enumerable.Empty<Models.Accounts.AccountEventModel>();
            }
            return response.Data;
        }

        public async Task<bool> SubscribeToEventAsync(Models.Accounts.AccountEventModel @event)
        {
            var request = this.createRequest(ApiUrls.AccountEventUrl, Method.PUT);
            request.AddBody(@event);

            var response = client.Execute(request);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<bool> UnSubscribeToEventAsync(Models.Accounts.AccountEventModel @event)
        {
            var request = this.createRequest(ApiUrls.AccountUnSubscribeEventUrl, Method.DELETE)
                .AddUrlSegment("id", @event.Id.ToString())
                ;

            var response = client.Execute(request);

            return this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<bool> SetEventFrequencyAsync(Models.Accounts.NotificationFrequency frequency)
        {
            var request = this.createRequest(ApiUrls.AccountEventFrequencyUrl, Method.PUT)
                .AddUrlSegment("frequency", frequency.ToString())
                ;

            var response = client.Execute(request);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }
    }
}
