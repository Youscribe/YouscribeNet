using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouScribe.Rest.Models.Libraries;
using System.Threading.Tasks;

namespace YouScribe.Rest
{
    class LibraryRequest : YouScribeRequest, ILibraryRequest
    {
        public LibraryRequest(IRestClient client, string authorizeToken)
            : base(client, authorizeToken)
        { }

        public IEnumerable<SimpleLibraryModel> Get()
        {
			return this.GetAsync().Result;
        }

		public Task<IEnumerable<SimpleLibraryModel>> GetAsync()
		{
			var tcs = new TaskCompletionSource<IEnumerable<SimpleLibraryModel>>();
			var request = this.createRequest(ApiUrls.LibraryUrl, Method.GET);
			var handler = client.ExecuteAsync<List<SimpleLibraryModel>>(request, (response) =>
				{
					if (response.StatusCode != System.Net.HttpStatusCode.OK)
					{
						this.addErrors(response);
						tcs.SetResult(null);
					}
					tcs.SetResult(response.Data);
				});

			return tcs.Task;
		}

		public Task<Models.Libraries.LibraryModel> GetAsync(int id)
		{
			var tcs = new TaskCompletionSource<Models.Libraries.LibraryModel>();
			var request = this.createRequest(ApiUrls.LibraryGetUrl, Method.GET)
				.AddUrlSegment("id", id.ToString());
			client.ExecuteAsync<LibraryModel>(request, (response) => {
				if (response.StatusCode != System.Net.HttpStatusCode.OK)
				{
					this.addErrors(response);
						tcs.SetResult(null);
				}
				tcs.SetResult(response.Data);
			});
			return tcs.Task;
		}

        public Models.Libraries.LibraryModel Get(int id)
        {
			return this.GetAsync(id).Result;
        }

        public Models.Libraries.LibraryModel Get(string typeName)
        {
            var request = this.createRequest(ApiUrls.LibraryGetByTypeNameUrl, Method.GET)
                .AddUrlSegment("typeName", typeName);
            var response = client.Execute<LibraryModel>(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                this.addErrors(response);
                return null;
            }
            return response.Data;
        }

        public bool AddProduct(int id, int productId)
        {
            var request = this.createRequest(ApiUrls.LibraryGetByTypeNameUrl, Method.PUT)
                .AddUrlSegment("id", id.ToString())
                .AddUrlSegment("productId", productId.ToString());
            var response = client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }

        public bool AddProduct(string typeName, int productId)
        {
            var request = this.createRequest(ApiUrls.LibraryGetByTypeNameUrl, Method.PUT)
                .AddUrlSegment("typeName", typeName)
                .AddUrlSegment("productId", productId.ToString());
            var response = client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }

        public bool DeleteProduct(int id, int productId)
        {
            var request = this.createRequest(ApiUrls.LibraryGetByTypeNameUrl, Method.DELETE)
                .AddUrlSegment("id", id.ToString())
                .AddUrlSegment("productId", productId.ToString());
            var response = client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }

        public bool DeleteProduct(string typeName, int productId)
        {
            var request = this.createRequest(ApiUrls.LibraryGetByTypeNameUrl, Method.DELETE)
                .AddUrlSegment("typeName", typeName)
                .AddUrlSegment("productId", productId.ToString());
            var response = client.Execute(request);

            return this.handleResponse(response, System.Net.HttpStatusCode.NoContent);
        }

        public IEnumerable<int> GetByProductId(int productId)
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
