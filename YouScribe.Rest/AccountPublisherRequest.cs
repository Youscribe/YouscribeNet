using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace YouScribe.Rest
{
    class AccountPublisherRequest : YouScribeRequest, IAccountPublisherRequest
    {
        public AccountPublisherRequest(IRestClient client, string authorizeToken)
            : base(client, authorizeToken)
        { }

        public bool SetAsPaypalPublisher(Models.Accounts.AccountPublisherPaypalModel paypalPublisher)
        {
            var request = this.createRequest(ApiUrls.AccountPaypalPublisherUrl, Method.PUT);

            request.AddBody(paypalPublisher);

            var response = this.client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }

        public bool SetAsTransferPublisher(Models.Accounts.AccountPublisherTransferModel transferPublisher)
        {
            var request = this.createRequest(ApiUrls.AccountTransferPublisherUrl, Method.PUT);

            request.AddBody(transferPublisher);

            var response = this.client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }
    }
}
