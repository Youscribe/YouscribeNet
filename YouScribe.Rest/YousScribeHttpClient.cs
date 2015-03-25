using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Youscribe.Rest.Cryptography;
using System.Threading.Tasks;
using System.Threading;

namespace YouScribe.Rest
{
    public class YousScribeHttpClient : IYousScribeHttpClient
    {


        static public Func<Func<HttpMessageHandler>, IYousScribeHttpClient> GetBaseClientFactory()
        {
            return (c) =>
            {
                return c == null ? new YousScribeHttpClient(new HttpClient()) : new YousScribeHttpClient(new HttpClient(c()));
            };
        }

        public HttpClient BaseClient
        {
            get;
            private set;
        }

        public Uri BaseAddress
        {
            get { return BaseClient.BaseAddress; }
            set { BaseClient.BaseAddress = value; }
        }
        public long MaxResponseContentBufferSize
        {
            get { return BaseClient.MaxResponseContentBufferSize; }
            set { BaseClient.MaxResponseContentBufferSize = value; }
        }
        public HttpRequestHeaders DefaultRequestHeaders
        {
            get { return BaseClient.DefaultRequestHeaders; }
        }
        public TimeSpan Timeout
        {
            get { return BaseClient.Timeout; }
            set { BaseClient.Timeout = value; }
        }

        public YousScribeHttpClient(HttpClient baseClient)
        {
            this.BaseClient = baseClient;
        }

        public HttpRequestHeaders GetDefaultHeaders()
        {
            return this.BaseClient.DefaultRequestHeaders;
        }
        public TimeSpan GetTimeOut()
        {
            return this.BaseClient.Timeout;
        }

        // Summary:
        //     Cancel all pending requests on this instance.
        public virtual void CancelPendingRequests()
        {
            this.BaseClient.CancelPendingRequests();
        }
        //
        // Summary:
        //     Send a DELETE request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        public virtual async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return await this.BaseClient.DeleteAsync(requestUri);
        }
        //
        // Summary:
        //     Send a DELETE request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        public virtual async Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            return await this.BaseClient.DeleteAsync(requestUri);
        }
        public virtual async Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
        {
            return await this.BaseClient.DeleteAsync(requestUri, cancellationToken);
        }
        public virtual async Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return await this.BaseClient.DeleteAsync(requestUri, cancellationToken);
        }
        //
        // Summary:
        //     Send a GET request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        public virtual async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await BaseClient.GetAsync(requestUri);
        }
        //
        // Summary:
        //     Send a GET request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        public virtual async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return await BaseClient.GetAsync(requestUri);
        }
        public virtual async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
        {
            return await BaseClient.GetAsync(requestUri, cancellationToken);
        }
        public virtual async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption)
        {
            return await BaseClient.GetAsync(requestUri, completionOption);
        }
        public virtual async Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return await BaseClient.GetAsync(requestUri, cancellationToken);
        }
        public virtual async Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)
        {
            return await BaseClient.GetAsync(requestUri, completionOption);
        }
        public virtual async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return await BaseClient.GetAsync(requestUri, completionOption, cancellationToken);
        }
        public virtual async Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return await BaseClient.GetAsync(requestUri, completionOption, cancellationToken);
        }
        

        //
        // Summary:
        //     Send a POST request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   content:
        //     The HTTP request content sent to the server.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        public virtual async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return await BaseClient.PostAsync(requestUri, content);
        }
        //
        // Summary:
        //     Send a POST request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   content:
        //     The HTTP request content sent to the server.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        public virtual async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            return await BaseClient.PostAsync(requestUri, content);
        }
        public virtual async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return await BaseClient.PostAsync(requestUri, content, cancellationToken);
        }
        public virtual async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return await BaseClient.PostAsync(requestUri, content, cancellationToken);
        }
        //
        // Summary:
        //     Send a PUT request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   content:
        //     The HTTP request content sent to the server.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        public virtual async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            return await BaseClient.PutAsync(requestUri, content);
        }
        //
        // Summary:
        //     Send a PUT request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        //   content:
        //     The HTTP request content sent to the server.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        public virtual async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
        {
            return await BaseClient.PutAsync(requestUri, content);
        }
        public virtual async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return await BaseClient.PutAsync(requestUri, content, cancellationToken);
        }
        public virtual async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return await BaseClient.PutAsync(requestUri, content, cancellationToken);
        }
        //
        // Summary:
        //     Send an HTTP request as an asynchronous operation.
        //
        // Parameters:
        //   request:
        //     The HTTP request message to send.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return await BaseClient.SendAsync(request);
        }
        //
        // Summary:
        //     Send an HTTP request as an asynchronous operation.
        //
        // Parameters:
        //   request:
        //     The HTTP request message to send.
        //
        //   cancellationToken:
        //     The cancellation token to cancel operation.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await BaseClient.SendAsync(request, cancellationToken);
        }
        //
        // Summary:
        //     Send an HTTP request as an asynchronous operation.
        //
        // Parameters:
        //   request:
        //     The HTTP request message to send.
        //
        //   completionOption:
        //     When the operation should complete (as soon as a response is available or
        //     after reading the whole response content).
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     This operation will not block. The request message was already sent by the
        //     System.Net.Http.HttpClient instance.
        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            return await BaseClient.SendAsync(request, completionOption);
        }
        //
        // Summary:
        //     Send an HTTP request as an asynchronous operation.
        //
        // Parameters:
        //   request:
        //     The HTTP request message to send.
        //
        //   completionOption:
        //     When the operation should complete (as soon as a response is available or
        //     after reading the whole response content).
        //
        //   cancellationToken:
        //     The cancellation token to cancel operation.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        //
        // Exceptions:
        //   System.InvalidOperationException:
        //     The request message was already sent by the System.Net.Http.HttpClient instance.
        public virtual async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return await BaseClient.SendAsync(request, completionOption, cancellationToken);
        }

    }
}
