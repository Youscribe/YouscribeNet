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
    public class YousScribeHttpClient : HttpClient
    {
        protected HttpClient baseClient;

        public HttpClient BaseClient {
            get {return baseClient;}
        }

        static public Func<Func<HttpMessageHandler>, YousScribeHttpClient> GetBaseClientFactory()
        {
            return (c) =>
            {
                return c == null ? new YousScribeHttpClient(new HttpClient()) : new YousScribeHttpClient(new HttpClient(c()));
            };
        }

        public YousScribeHttpClient(HttpClient baseClient)
        {
            this.baseClient = baseClient;
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
        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await baseClient.GetAsync(requestUri);
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
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return await baseClient.GetAsync(requestUri);
        }
        public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
        {
            return await baseClient.GetAsync(requestUri, cancellationToken);
        }
        public async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption)
        {
            return await baseClient.GetAsync(requestUri, completionOption);
        }
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return await baseClient.GetAsync(requestUri, cancellationToken);
        }
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)
        {
            return await baseClient.GetAsync(requestUri, completionOption);
        }
        public async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return await baseClient.GetAsync(requestUri, completionOption, cancellationToken);
        }
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return await baseClient.GetAsync(requestUri, completionOption, cancellationToken);
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
            return await baseClient.PostAsync(requestUri, content);
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
            return await baseClient.PostAsync(requestUri, content);
        }
        public virtual async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return await baseClient.PostAsync(requestUri, content, cancellationToken);
        }
        public virtual async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return await baseClient.PostAsync(requestUri, content, cancellationToken);
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
            return await baseClient.PutAsync(requestUri, content);
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
            return await baseClient.PutAsync(requestUri, content);
        }
        public virtual async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return await baseClient.PutAsync(requestUri, content, cancellationToken);
        }
        public virtual async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return await baseClient.PutAsync(requestUri, content, cancellationToken);
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
            return await baseClient.SendAsync(request);
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
            return await baseClient.SendAsync(request, cancellationToken);
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
            return await baseClient.SendAsync(request, completionOption);
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
            return await baseClient.SendAsync(request, completionOption, cancellationToken);
        }

    }
}
