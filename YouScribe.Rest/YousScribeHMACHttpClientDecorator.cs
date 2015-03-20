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
    class YousScribeHMACHttpClientDecorator : YousScribeHttpClient
    {
        byte[] secretKey;
        int applicationId;
        Random random;
        byte[] randBytes;
        IHMAC hmac;

        static public Func<Func<HttpMessageHandler>, YousScribeHttpClient> GetBaseClientFactory(byte[] secretKey, int applicationId, IHMAC hmac)
        {
            return (c) =>
            {
                return c == null ? new YousScribeHMACHttpClientDecorator(new HttpClient(), secretKey, applicationId, hmac) : new YousScribeHMACHttpClientDecorator(new HttpClient(c()), secretKey, applicationId, hmac);
            };
        }

        private YousScribeHMACHttpClientDecorator(HttpClient baseClient, byte[] secretKey, int applicationId, IHMAC hmac)
            : base(baseClient)
        {
            this.secretKey = secretKey;
            this.applicationId = applicationId;
            this.random = new Random();
            this.randBytes = new byte[8];
            this.hmac = hmac;
        }

        byte[] StringEncode(string text)
        {
            var encoding = new System.Text.UnicodeEncoding();
            return encoding.GetBytes(text);
        }
        void addHMACHeaders(Uri uri)
        {
            random.NextBytes(randBytes);
            String randomKey = Convert.ToBase64String(randBytes);
            DateTimeOffset dateNow = DateTimeOffset.Now;

            String uriString = uri.Host + uri.AbsolutePath;

            byte[] message = StringEncode(uriString + dateNow.ToString() + randomKey);

            byte[] signature = hmac.ComputeHash(message, secretKey);

            this.baseClient.DefaultRequestHeaders.Add(ApiUrls.HMACAuthenticateRandomKeyHeader, randomKey);
            this.baseClient.DefaultRequestHeaders.Date = dateNow;
            this.baseClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ApiUrls.HMACScheme, applicationId + ":" + Convert.ToBase64String(signature));
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
            addHMACHeaders(new Uri(requestUri));
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
            addHMACHeaders(requestUri);
            return await baseClient.GetAsync(requestUri);
        }
        public async Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
        {
            addHMACHeaders(new Uri(requestUri));
            return await baseClient.GetAsync(requestUri, cancellationToken);
        }
        public async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption)
        {
            addHMACHeaders(new Uri(requestUri));
            return await baseClient.GetAsync(requestUri, completionOption);
        }
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            addHMACHeaders(requestUri);
            return await baseClient.GetAsync(requestUri, cancellationToken);
        }
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)
        {
            addHMACHeaders(requestUri);
            return await baseClient.GetAsync(requestUri, completionOption);
        }
        public async Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            addHMACHeaders(new Uri(requestUri));
            return await baseClient.GetAsync(requestUri, completionOption, cancellationToken);
        }
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            addHMACHeaders(requestUri);
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
        public override async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            addHMACHeaders(new Uri(requestUri));
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
        public override async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            addHMACHeaders(requestUri);
            return await baseClient.PostAsync(requestUri, content);
        }
        public override async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            addHMACHeaders(new Uri(requestUri));
            return await baseClient.PostAsync(requestUri, content, cancellationToken);
        }
        public override async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            addHMACHeaders(requestUri);
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
        public override async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            addHMACHeaders(new Uri(requestUri));
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
        public override async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
        {
            addHMACHeaders(requestUri);
            return await baseClient.PutAsync(requestUri, content);
        }
        public override async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            addHMACHeaders(new Uri(requestUri));
            return await baseClient.PutAsync(requestUri, content, cancellationToken);
        }
        public override async Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            addHMACHeaders(requestUri);
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
        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            addHMACHeaders(request.RequestUri);
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
        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            addHMACHeaders(request.RequestUri);
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
        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
        {
            addHMACHeaders(request.RequestUri);
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
        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            addHMACHeaders(request.RequestUri);
            return await baseClient.SendAsync(request, completionOption, cancellationToken);
        }

    }
}
