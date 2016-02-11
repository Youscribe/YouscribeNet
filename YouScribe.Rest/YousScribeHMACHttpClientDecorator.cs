using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using YouScribe.Rest.Cryptography;
using System.Threading.Tasks;
using System.Threading;

namespace YouScribe.Rest
{
    public class YousScribeHMACHttpClientDecorator : YousScribeHttpClient
    {
        byte[] secretKey;
        int applicationId;
        Random random;
        byte[] randBytes;
        IHMAC hmac;

        static public Func<Func<HttpMessageHandler>, IYousScribeHttpClient> GetBaseClientFactory(byte[] secretKey, int applicationId, IHMAC hmac)
        {
            return (c) =>
            {
                return c == null ? new YousScribeHMACHttpClientDecorator(new HttpClient(), secretKey, applicationId, hmac) : new YousScribeHMACHttpClientDecorator(new HttpClient(c()), secretKey, applicationId, hmac);
            };
        }

        private YousScribeHMACHttpClientDecorator(HttpClient BaseClient, byte[] secretKey, int applicationId, IHMAC hmac)
            : base(BaseClient)
        {
            this.secretKey = secretKey;
            this.applicationId = applicationId;
            this.random = new Random();
            this.randBytes = new byte[8];
            this.hmac = hmac;
        }

        byte[] StringEncode(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        void addHMACHeaders(Uri uri)
        {
            random.NextBytes(randBytes);
            var randomKey = Convert.ToBase64String(randBytes);
            DateTimeOffset dateNow = DateTimeOffset.Now;

            var uriString = uri.Host + uri.AbsolutePath;
            this.BaseClient.DefaultRequestHeaders.Date = dateNow;

            var date = this.BaseClient.DefaultRequestHeaders.GetValues("Date").FirstOrDefault();

            byte[] message = StringEncode(uriString + date + randomKey);
            byte[] signature = hmac.ComputeHash(message, secretKey);

            this.BaseClient.DefaultRequestHeaders.Add(ApiUrls.HMACAuthenticateRandomKeyHeader, randomKey);
            this.BaseClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ApiUrls.HMACScheme, applicationId + ":" + Convert.ToBase64String(signature));
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
        public override Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            addHMACHeaders(new Uri(requestUri));
            return this.BaseClient.DeleteAsync(requestUri);
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
        public override Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            addHMACHeaders(requestUri);
            return this.BaseClient.DeleteAsync(requestUri);
        }
        public override Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
        {
            addHMACHeaders(new Uri(requestUri));
            return this.BaseClient.DeleteAsync(requestUri, cancellationToken);
        }
        public override Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            addHMACHeaders(requestUri);
            return this.BaseClient.DeleteAsync(requestUri, cancellationToken);
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
        public override Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            addHMACHeaders(new Uri(requestUri));
            return BaseClient.GetAsync(requestUri);
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
        public override Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            addHMACHeaders(requestUri);
            return BaseClient.GetAsync(requestUri);
        }
        public override Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
        {
            addHMACHeaders(new Uri(requestUri));
            return BaseClient.GetAsync(requestUri, cancellationToken);
        }
        public override Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption)
        {
            addHMACHeaders(new Uri(requestUri));
            return BaseClient.GetAsync(requestUri, completionOption);
        }
        public override Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            addHMACHeaders(requestUri);
            return BaseClient.GetAsync(requestUri, cancellationToken);
        }
        public override Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)
        {
            addHMACHeaders(requestUri);
            return BaseClient.GetAsync(requestUri, completionOption);
        }
        public override Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            addHMACHeaders(new Uri(requestUri));
            return BaseClient.GetAsync(requestUri, completionOption, cancellationToken);
        }
        public override Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            addHMACHeaders(requestUri);
            return BaseClient.GetAsync(requestUri, completionOption, cancellationToken);
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
        public override Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            addHMACHeaders(new Uri(requestUri));
            return BaseClient.PostAsync(requestUri, content);
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
        public override Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            addHMACHeaders(requestUri);
            return BaseClient.PostAsync(requestUri, content);
        }
        public override Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            addHMACHeaders(new Uri(requestUri));
            return BaseClient.PostAsync(requestUri, content, cancellationToken);
        }
        public override Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            addHMACHeaders(requestUri);
            return BaseClient.PostAsync(requestUri, content, cancellationToken);
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
        public override Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            addHMACHeaders(new Uri(requestUri));
            return BaseClient.PutAsync(requestUri, content);
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
        public override Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
        {
            addHMACHeaders(requestUri);
            return BaseClient.PutAsync(requestUri, content);
        }
        public override Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            addHMACHeaders(new Uri(requestUri));
            return BaseClient.PutAsync(requestUri, content, cancellationToken);
        }
        public override Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            addHMACHeaders(requestUri);
            return BaseClient.PutAsync(requestUri, content, cancellationToken);
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
        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            addHMACHeaders(request.RequestUri);
            return BaseClient.SendAsync(request);
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
        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            addHMACHeaders(request.RequestUri);
            return BaseClient.SendAsync(request, cancellationToken);
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
        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            addHMACHeaders(request.RequestUri);
            return BaseClient.SendAsync(request, completionOption);
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
        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            addHMACHeaders(request.RequestUri);
            return BaseClient.SendAsync(request, completionOption, cancellationToken);
        }

    }
}
