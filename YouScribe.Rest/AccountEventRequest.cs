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
            using (var client = this.CreateClient())
            {
                var response = await client.GetAsync(ApiUrls.AccountEventUrl);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await this.AddErrorsAsync(response);
                    return Enumerable.Empty<Models.Accounts.AccountEventModel>();
                }
                return response.Data;
            }
        }

        public async Task<bool> SubscribeToEventAsync(Models.Accounts.AccountEventModel @event)
        {
            using (var client = this.CreateClient())
            {
                var content = this.GetContent(@event);
                var response = await client.PutAsync(ApiUrls.AccountEventUrl, content);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }

        public async Task<bool> UnSubscribeToEventAsync(Models.Accounts.AccountEventModel @event)
        {
            using (var client = this.CreateClient())
            {
                var content = this.GetContent(@event);
                var url = ApiUrls.AccountUnSubscribeEventUrl.Replace("{id}", @event.Id.ToString());
                var response = await client.DeleteAsync(url, content);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }

        public async Task<bool> SetEventFrequencyAsync(Models.Accounts.NotificationFrequency frequency)
        {
            using (var client = this.CreateClient())
            {
                var url = ApiUrls.AccountEventFrequencyUrl.Replace("{frequency}", frequency.ToString());
                var response = await client.PutAsync(url, null);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }
    }
}
