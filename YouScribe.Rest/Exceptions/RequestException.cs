using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Exceptions
{

    public class RequestException : Exception
    {
        public int StatusCode { get; set; }

        public string BaseUrl { get; set; }

        public RequestException() { }
        public RequestException(string message, int statusCode, string baseUrl) : base(message) { this.StatusCode = statusCode; this.BaseUrl = baseUrl; }
        public RequestException(string message, Exception inner) : base(message, inner) { }
    }
}
