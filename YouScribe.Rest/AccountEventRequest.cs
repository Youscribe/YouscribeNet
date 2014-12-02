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
            var client = this.CreateClient();
            var response = await client.GetAsync(this.GetUri(ApiUrls.AccountEventUrl)).ConfigureAwait(false);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await this.AddErrorsAsync(response).ConfigureAwait(false);
                return Enumerable.Empty<Models.Accounts.AccountEventModel>();
            }
            return await this.GetObjectAsync<IEnumerable<Models.Accounts.AccountEventModel>>(response.Content).ConfigureAwait(false);
        }

        public async Task<bool> SubscribeToEventAsync(string name)
        {
            var client = this.CreateClient();
            var url = ApiUrls.AccountSubscribeEventUrl;
            url = url.Replace("{name}", name);
            var response = await client.PutAsync(this.GetUri(url), null).ConfigureAwait(false);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
        }

        public async Task<bool> UnSubscribeToEventAsync(Models.Accounts.AccountEventModel @event)
        {
            var client = this.CreateClient();
            var url = ApiUrls.AccountUnSubscribeEventUrl.Replace("{id}", @event.Id.ToString());
            var response = await client.DeleteAsync(this.GetUri(url)).ConfigureAwait(false);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
        }

        public async Task<bool> SetEventFrequencyAsync(Models.Accounts.NotificationFrequency frequency)
        {
            var client = this.CreateClient();
            var url = ApiUrls.AccountEventFrequencyUrl.Replace("{frequency}", frequency.ToString());
            var response = await client.PutAsync(this.GetUri(url), null).ConfigureAwait(false);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
        }
    }
}
