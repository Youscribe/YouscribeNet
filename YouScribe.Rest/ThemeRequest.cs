using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest
{
    class ThemeRequest : YouScribeRequest, IThemeRequest
    {
        public ThemeRequest(Func<DisposableClient> clientFactory, ITokenProvider authorizeTokenProvider)
            : base(clientFactory, authorizeTokenProvider)
        { }

        public async System.Threading.Tasks.Task<IEnumerable<Models.Products.ThemeModel>> GetAsync()
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var response = await client.GetAsync(this.GetUri(ApiUrls.ThemeUrl)).ConfigureAwait(false);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    await this.AddErrorsAsync(response).ConfigureAwait(false);
                    return Enumerable.Empty<ThemeModel>();
                }
                return await this.GetObjectAsync<IEnumerable<ThemeModel>>(response.Content).ConfigureAwait(false);
            }
        }
    }
}
