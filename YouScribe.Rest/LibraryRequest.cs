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
            using (var client = this.CreateClient())
            {
                var response = await client.GetAsync(ApiUrls.LibraryUrl);
                if (!response.IsSuccessStatusCode)
                {
                    await this.AddErrorsAsync(response);
                    return null;
                }
                model = response.Content.ReadAsAsync<List<SimpleLibraryModel>>();
            }
            return model;
		}

		public async Task<Models.Libraries.LibraryModel> GetAsync(int id)
		{
            IEnumerable<LibraryModel> model;
            using (var client = this.CreateClient())
            {
                var url = ApiUrls.LibraryGetUrl.Replace("{id}", id.ToString());
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    await this.AddErrorsAsync(response);
                    return null;
                }
                model = response.Content.ReadAsAsync<List<LibraryModel>>();
            }
            return model;
		}

        public async Task<Models.Libraries.LibraryModel> GetAsync(string typeName)
        {
            LibraryModel model = null;
            using (var client = this.CreateClient())
            {
                var url = ApiUrls.LibraryGetByTypeNameUrl.Replace("{typeName}", typeName.ToString());
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    await this.AddErrorsAsync(response);
                    return null;
                }
                model = response.Content.ReadAsAsync<LibraryModel>();
            }
            return model;
        }

        public async Task<bool> AddProductAsync(int id, int productId)
        {
            using (var client = this.CreateClient())
            {
                var url = ApiUrls.LibraryGetUrl.Replace("{id}", id.ToString())
                    .Replace("{productId}", productId.ToString());
                var response = await client.PutAsync(url, null);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }

        public async Task<bool> AddProductAsync(string typeName, int productId)
        {
            using (var client = this.CreateClient())
            {
                var url = ApiUrls.LibraryGetByTypeNameUrl.Replace("{typeName}", typeName.ToString())
                    .Replace("{productId}", productId.ToString());
                var response = await client.PutAsync(url, null);

                return await this.HandleResponseAsync(response, System.Net.HttpStatusCode.NoContent);
            }
        }

        public async Task<bool> DeleteProductAsync(int id, int productId)
        {
            var request = this.createRequest(ApiUrls.LibraryGetByTypeNameUrl, Method.DELETE)
                .AddUrlSegment("id", id.ToString())
                .AddUrlSegment("productId", productId.ToString());
            var response = client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<bool> DeleteProductAsync(string typeName, int productId)
        {
            var request = this.createRequest(ApiUrls.LibraryGetByTypeNameUrl, Method.DELETE)
                .AddUrlSegment("typeName", typeName)
                .AddUrlSegment("productId", productId.ToString());
            var response = client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }

        public async Task<IEnumerable<int>> GetByProductIdAsync(int productId)
        {
            var request = this.createRequest(ApiUrls.LibraryGetByProductIdUrl, Method.GET)
                .AddUrlSegment("productId", productId.ToString());
            var response = client.Execute<List<int>>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                this.addErrors(response);
                return null;
            }
            return response.Data;
        }
    }
}
