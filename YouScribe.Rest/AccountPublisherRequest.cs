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
            var request = this.createRequest(ApiUrls.AccountPaypalPublisherUrl, Method.PUT);

            request.AddBody(paypalPublisher);

            var response = this.client.Execute(request);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<bool> SetAsTransferPublisherAsync(Models.Accounts.AccountPublisherTransferModel transferPublisher)
        {
            var request = this.createRequest(ApiUrls.AccountTransferPublisherUrl, Method.PUT);

            request.AddBody(transferPublisher);

            var response = this.client.Execute(request);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }
    }
}
