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
        public LibraryRequest(Func<DisposableClient> clientFactory, ITokenProvider authorizeTokenProvider)
            : base(clientFactory, authorizeTokenProvider)
        { }

		public Task<IEnumerable<SimpleLibraryModel>> GetAsync()
		{
            return this.GetEnumerableAsync<SimpleLibraryModel>(ApiUrls.LibraryUrl);
		}

		public Task<Models.Libraries.LibraryModel> GetAsync(int id)
		{
            var url = ApiUrls.LibraryGetUrl.Replace("{id}", id.ToString());
            return this.GetAsync<Models.Libraries.LibraryModel>(url);
		}

        public Task<Models.Libraries.LibraryModel> GetAsync(string typeName)
        {
            var url = ApiUrls.LibraryGetByTypeNameUrl.Replace("{typeName}", typeName.ToString());
            return this.GetAsync<Models.Libraries.LibraryModel>(url);
        }

        public async Task<bool> AddProductAsync(int id, int productId, bool isPublic)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.LibraryAddProductUrl.Replace("{id}", id.ToString())
                    .Replace("{productId}", productId.ToString());
                var content = this.GetContent(isPublic);
                var response = await client.PutAsync(this.GetUri(url), content).ConfigureAwait(false);

                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<bool> AddProductAsync(string typeName, int productId, bool isPublic)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.LibraryAddByTypeNameProductUrl.Replace("{typeName}", typeName.ToString())
                    .Replace("{productId}", productId.ToString());
                var content = this.GetContent(isPublic);
                var response = await client.PutAsync(this.GetUri(url), content).ConfigureAwait(false);

                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<bool> DeleteProductAsync(int id, int productId)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.LibraryDeleteProductUrl.Replace("{id}", id.ToString())
                    .Replace("{productId}", productId.ToString());
                var response = await client.DeleteAsync(this.GetUri(url)).ConfigureAwait(false);

                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }

        public async Task<bool> DeleteProductAsync(string typeName, int productId)
        {
            using (var dclient = this.CreateClient())
            {
                var client = dclient.Client;
                var url = ApiUrls.LibraryDeleteByTypeNameProductUrl.Replace("{typeName}", typeName)
                    .Replace("{productId}", productId.ToString());
                var response = await client.DeleteAsync(this.GetUri(url)).ConfigureAwait(false);

                return await this.HandleResponseAsync(response).ConfigureAwait(false);
            }
        }

        public Task<IEnumerable<int>> GetByProductIdAsync(int productId)
        {
            var url = ApiUrls.LibraryGetByProductIdUrl.Replace("{productId}", productId.ToString());
            return this.GetEnumerableAsync<int>(url);
        }
    }
}
