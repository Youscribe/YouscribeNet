using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest
{
    public class TokenContext : IDisposable
    {
        IYouScribeRequest request;
        string oldToken;

        public TokenContext(IYouScribeRequest request, string authorizeToken)
        {
            this.request = request;
            oldToken = this.request.GetToken();
            this.request.SetToken(authorizeToken);
        }

        public void Dispose()
        {
            this.request.SetToken(oldToken);
        }
    }
}
