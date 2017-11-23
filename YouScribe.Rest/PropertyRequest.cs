using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest
{
    class PropertyRequest : YouScribeRequest, IPropertyRequest
    {
        public PropertyRequest(Func<DisposableClient> clientFactory, ITokenProvider authorizeTokenProvider)
            : base(clientFactory, authorizeTokenProvider)
        { }

        public async System.Threading.Tasks.Task<IEnumerable<Models.Products.PropertyModel>> GetAsync(string type)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;

                var uri = this.GetUri(ApiUrls.PropertyUrl.Replace("{type}", type));
                var response = await client.GetAsync(uri).ConfigureAwait(false);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await this.AddErrorsAsync(response).ConfigureAwait(false);
                    return Enumerable.Empty<PropertyModel>();
                }
                return await this.GetObjectAsync<IEnumerable<PropertyModel>>(response.Content).ConfigureAwait(false);
            }
        }
    }
}
