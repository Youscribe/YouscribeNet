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
            var response = await client.GetAsync(this.GetUri(ApiUrls.LibraryUrl));
            if (!response.IsSuccessStatusCode)
            {
                await this.AddErrorsAsync(response);
                return null;
            }
            model = await this.GetObjectAsync<IEnumerable<SimpleLibraryModel>>(response.Content);
            return model;
		}

		public async Task<Models.Libraries.LibraryModel> GetAsync(int id)
		{
            LibraryModel model;
            var client = this.CreateClient();
            var url = ApiUrls.LibraryGetUrl.Replace("{id}", id.ToString());
            var response = await client.GetAsync(this.GetUri(url));
            if (!response.IsSuccessStatusCode)
            {
                await this.AddErrorsAsync(response);
                return null;
            }
            model = await this.GetObjectAsync<LibraryModel>(response.Content);
            return model;
		}

        public async Task<Models.Libraries.LibraryModel> GetAsync(string typeName)
        {
            LibraryModel model = null;
            var client = this.CreateClient();
            var url = ApiUrls.LibraryGetByTypeNameUrl.Replace("{typeName}", typeName.ToString());
            var response = await client.GetAsync(this.GetUri(url));
            if (!response.IsSuccessStatusCode)
            {
                await this.AddErrorsAsync(response);
                return null;
            }
            model = await this.GetObjectAsync<LibraryModel>(response.Content);
            return model;
        }

        public async Task<bool> AddProductAsync(int id, int productId, bool isPublic)
        {
            var client = this.CreateClient();
            var url = ApiUrls.LibraryAddProductUrl.Replace("{id}", id.ToString())
                .Replace("{productId}", productId.ToString());
            var content = this.GetContent(isPublic);
            var response = await client.PutAsync(this.GetUri(url), content);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<bool> AddProductAsync(string typeName, int productId, bool isPublic)
        {
            var client = this.CreateClient();
            var url = ApiUrls.LibraryAddByTypeNameProductUrl.Replace("{typeName}", typeName.ToString())
                .Replace("{productId}", productId.ToString());
            var content = this.GetContent(isPublic);
            var response = await client.PutAsync(this.GetUri(url), content);

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<bool> DeleteProductAsync(int id, int productId)
        {
            var client = this.CreateClient();
            var url = ApiUrls.LibraryGetUrl.Replace("{id}", id.ToString())
                .Replace("{productId}", productId.ToString());
            var response = await client.DeleteAsync(this.GetUri(url));

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<bool> DeleteProductAsync(string typeName, int productId)
        {
            var client = this.CreateClient();
            var url = ApiUrls.LibraryGetByTypeNameUrl.Replace("{typeName}", typeName.ToString())
                .Replace("{productId}", productId.ToString());
            var response = await client.DeleteAsync(this.GetUri(url));

            return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<IEnumerable<int>> GetByProductIdAsync(int productId)
        {
            var client = this.CreateClient();
            var url = ApiUrls.LibraryGetByProductIdUrl.Replace("{productId}", productId.ToString());
            var response = await client.GetAsync(this.GetUri(url));

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await this.AddErrorsAsync(response);
                return null;
            }
            return await this.GetObjectAsync<IEnumerable<int>>(response.Content);
        }
    }
}
