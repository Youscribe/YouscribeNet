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
        public AccountEventRequest(Func<DisposableClient> clientFactory, ITokenProvider authorizeTokenProvider)
            : base(clientFactory, authorizeTokenProvider)
        { }

        public Task<IEnumerable<Models.Accounts.AccountEventModel>> ListAllEventsAsync()
        {
            return this.GetEnumerableAsync<Models.Accounts.AccountEventModel>(ApiUrls.AccountEventUrl);
        }

        public async Task<bool> SubscribeToEventAsync(string name)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.AccountSubscribeEventUrl;
                url = url.Replace("{name}", name);
                var response = await client.PutAsync(this.GetUri(url), null).ConfigureAwait(false);

                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<bool> UnSubscribeToEventAsync(Models.Accounts.AccountEventModel @event)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.AccountUnSubscribeEventUrl.Replace("{id}", @event.Id.ToString());
                var response = await client.DeleteAsync(this.GetUri(url)).ConfigureAwait(false);

                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<bool> SetEventFrequencyAsync(Models.Accounts.NotificationFrequency frequency)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.AccountEventFrequencyUrl.Replace("{frequency}", frequency.ToString());
                var response = await client.PutAsync(this.GetUri(url), null).ConfigureAwait(false);

                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }
    }
}
