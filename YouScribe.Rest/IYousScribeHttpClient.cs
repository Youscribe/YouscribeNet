using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
namespace YouScribe.Rest
{
    public interface IYousScribeHttpClient
    {

        HttpClient BaseClient
        {
            get;
        }

        Uri BaseAddress
        {
            get;
            set;
        }
        long MaxResponseContentBufferSize
        {
            get;
            set;
        }
        HttpRequestHeaders DefaultRequestHeaders
        {
            get;
        }
        TimeSpan Timeout
        {
            get;
            set;
        }
        void CancelPendingRequests();
        Task<HttpResponseMessage> DeleteAsync(string requestUri);
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri);
        Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken);
        Task<System.Net.Http.HttpResponseMessage> GetAsync(string requestUri);
        Task<System.Net.Http.HttpResponseMessage> GetAsync(string requestUri, System.Net.Http.HttpCompletionOption completionOption);
        Task<System.Net.Http.HttpResponseMessage> GetAsync(string requestUri, System.Net.Http.HttpCompletionOption completionOption, System.Threading.CancellationToken cancellationToken);
        Task<System.Net.Http.HttpResponseMessage> GetAsync(string requestUri, System.Threading.CancellationToken cancellationToken);
        Task<System.Net.Http.HttpResponseMessage> GetAsync(Uri requestUri);
        Task<System.Net.Http.HttpResponseMessage> GetAsync(Uri requestUri, System.Net.Http.HttpCompletionOption completionOption);
        Task<System.Net.Http.HttpResponseMessage> GetAsync(Uri requestUri, System.Net.Http.HttpCompletionOption completionOption, System.Threading.CancellationToken cancellationToken);
        Task<System.Net.Http.HttpResponseMessage> GetAsync(Uri requestUri, System.Threading.CancellationToken cancellationToken);
        Task<System.Net.Http.HttpResponseMessage> PostAsync(string requestUri, System.Net.Http.HttpContent content);
        Task<System.Net.Http.HttpResponseMessage> PostAsync(string requestUri, System.Net.Http.HttpContent content, System.Threading.CancellationToken cancellationToken);
        Task<System.Net.Http.HttpResponseMessage> PostAsync(Uri requestUri, System.Net.Http.HttpContent content);
        Task<System.Net.Http.HttpResponseMessage> PostAsync(Uri requestUri, System.Net.Http.HttpContent content, System.Threading.CancellationToken cancellationToken);
        Task<System.Net.Http.HttpResponseMessage> PutAsync(string requestUri, System.Net.Http.HttpContent content);
        Task<System.Net.Http.HttpResponseMessage> PutAsync(string requestUri, System.Net.Http.HttpContent content, System.Threading.CancellationToken cancellationToken);
        Task<System.Net.Http.HttpResponseMessage> PutAsync(Uri requestUri, System.Net.Http.HttpContent content);
        Task<System.Net.Http.HttpResponseMessage> PutAsync(Uri requestUri, System.Net.Http.HttpContent content, System.Threading.CancellationToken cancellationToken);
        Task<System.Net.Http.HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request);
        Task<System.Net.Http.HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, System.Net.Http.HttpCompletionOption completionOption);
        Task<System.Net.Http.HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, System.Net.Http.HttpCompletionOption completionOption, System.Threading.CancellationToken cancellationToken);
        Task<System.Net.Http.HttpResponseMessage> SendAsync(System.Net.Http.HttpRequestMessage request, System.Threading.CancellationToken cancellationToken);
    }
}
