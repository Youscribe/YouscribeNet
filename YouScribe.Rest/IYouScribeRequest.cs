using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest
{
    public class RequestError
    {
        YouScribeRequest request;

        internal RequestError(YouScribeRequest request)
        {
            this.request = request;
            this.Messages = Enumerable.Empty<string>();
        }

        public int StatusCode { get; set; }

        public IEnumerable<string> Messages { get; set; }

        public string RawOutput { get; set; }

        public T ParseError<T>()
        {
            if (this.RawOutput == null)
                return default(T);
            return this.request.GetObject<T>(this.RawOutput);
        }
    }

    public interface IYouScribeRequest
    {
        /// <summary>
        /// The errors return by the server
        /// </summary>
        RequestError Error { get; }
    }
}
