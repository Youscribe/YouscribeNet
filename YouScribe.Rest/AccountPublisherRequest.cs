using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    class AccountPublisherRequest : YouScribeRequest, IAccountPublisherRequest
    {
        public AccountPublisherRequest(Func<IYousScribeHttpClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

        public async Task<bool> SetAsPaypalPublisherAsync(Models.Accounts.AccountPublisherPaypalModel paypalPublisher)
        {
            var client = this.CreateClient();
            var content = this.GetContent(paypalPublisher);
            var response = await client.PutAsync(this.GetUri(ApiUrls.AccountPaypalPublisherUrl), content).ConfigureAwait(false);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
        }

        public async Task<bool> SetAsTransferPublisherAsync(Models.Accounts.AccountPublisherTransferModel transferPublisher)
        {
            var client = this.CreateClient();
            var content = this.GetContent(transferPublisher);
            var response = await client.PutAsync(this.GetUri(ApiUrls.AccountTransferPublisherUrl), content).ConfigureAwait(false);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
        }
    }
}
