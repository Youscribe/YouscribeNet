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
        public AccountPublisherRequest(Func<HttpClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

        public async Task<bool> SetAsPaypalPublisherAsync(Models.Accounts.AccountPublisherPaypalModel paypalPublisher)
        {
            using (var client = this.CreateClient())
            {
                var content = this.GetContent(paypalPublisher);
                var response = await client.PutAsync(ApiUrls.AccountPaypalPublisherUrl, content);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }

        public async Task<bool> SetAsTransferPublisherAsync(Models.Accounts.AccountPublisherTransferModel transferPublisher)
        {
            using (var client = this.CreateClient())
            {
                var content = this.GetContent(transferPublisher);
                var response = await client.PutAsync(ApiUrls.AccountTransferPublisherUrl, content);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }
    }
}
