using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Exceptions
{

    public class RequestException : Exception
    {
        public int StatusCode { get; set; }

        public RequestException() { }
        public RequestException(string message, int statusCode) : base(message) { this.StatusCode = statusCode; }
        public RequestException(string message, Exception inner) : base(message, inner) { }
    }
}
