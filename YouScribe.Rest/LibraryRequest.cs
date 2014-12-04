using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Libraries;
using System.Threading.Tasks;
using System.Net.Http;

namespace YouScribe.Rest
{
    class LibraryRequest : YouScribeRequest, ILibraryRequest
    {
        public LibraryRequest(Func<HttpClient> clientFactory, string authorizeToken)
            : base(clientFactory, authorizeToken)
        { }

		public async Task<IEnumerable<SimpleLibraryModel>> GetAsync()
		{
            IEnumerable<SimpleLibraryModel> model;
            var client = this.CreateClient();
            var response = await client.GetAsync(this.GetUri(ApiUrls.LibraryUrl)).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                await this.AddErrorsAsync(response).ConfigureAwait(false);
                return null;
            }
            model = await this.GetObjectAsync<IEnumerable<SimpleLibraryModel>>(response.Content).ConfigureAwait(false);
            return model;
		}

		public async Task<Models.Libraries.LibraryModel> GetAsync(int id)
		{
            LibraryModel model;
            var client = this.CreateClient();
            var url = ApiUrls.LibraryGetUrl.Replace("{id}", id.ToString());
            var response = await client.GetAsync(this.GetUri(url)).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                await this.AddErrorsAsync(response).ConfigureAwait(false);
                return null;
            }
            model = await this.GetObjectAsync<LibraryModel>(response.Content).ConfigureAwait(false);
            return model;
		}

        public async Task<Models.Libraries.LibraryModel> GetAsync(string typeName)
        {
            LibraryModel model = null;
            var client = this.CreateClient();
            var url = ApiUrls.LibraryGetByTypeNameUrl.Replace("{typeName}", typeName.ToString());
            var response = await client.GetAsync(this.GetUri(url)).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                await this.AddErrorsAsync(response).ConfigureAwait(false);
                return null;
            }
            model = await this.GetObjectAsync<LibraryModel>(response.Content).ConfigureAwait(false);
            return model;
        }

        public async Task<bool> AddProductAsync(int id, int productId, bool isPublic)
        {
            var client = this.CreateClient();
            var url = ApiUrls.LibraryAddProductUrl.Replace("{id}", id.ToString())
                .Replace("{productId}", productId.ToString());
            var content = this.GetContent(isPublic);
            var response = await client.PutAsync(this.GetUri(url), content).ConfigureAwait(false);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
        }

        public async Task<bool> AddProductAsync(string typeName, int productId, bool isPublic)
        {
            var client = this.CreateClient();
            var url = ApiUrls.LibraryAddByTypeNameProductUrl.Replace("{typeName}", typeName.ToString())
                .Replace("{productId}", productId.ToString());
            var content = this.GetContent(isPublic);
            var response = await client.PutAsync(this.GetUri(url), content).ConfigureAwait(false);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
        }

        public async Task<bool> DeleteProductAsync(int id, int productId)
        {
            var client = this.CreateClient();
			var url = ApiUrls.LibraryDeleteProductUrl.Replace("{id}", id.ToString())
                .Replace("{productId}", productId.ToString());
            var response = await client.DeleteAsync(this.GetUri(url)).ConfigureAwait(false);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
        }

        public async Task<bool> DeleteProductAsync(string typeName, int productId)
        {
            var client = this.CreateClient();
            var url = ApiUrls.LibraryGetByTypeNameUrl.Replace("{typeName}", typeName.ToString())
                .Replace("{productId}", productId.ToString());
            var response = await client.DeleteAsync(this.GetUri(url)).ConfigureAwait(false);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent).ConfigureAwait(false);
        }

        public async Task<IEnumerable<int>> GetByProductIdAsync(int productId)
        {
            var client = this.CreateClient();
            var url = ApiUrls.LibraryGetByProductIdUrl.Replace("{productId}", productId.ToString());
            var response = await client.GetAsync(this.GetUri(url)).ConfigureAwait(false);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await this.AddErrorsAsync(response).ConfigureAwait(false);
                return null;
            }
            return await this.GetObjectAsync<IEnumerable<int>>(response.Content).ConfigureAwait(false);
        }
    }
}
