using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest
{
    public class DefaultTokenProvider : ITokenProvider
    {
        public string token;

        public DefaultTokenProvider()
        {

        }

        public DefaultTokenProvider(string token)
        {
            this.token = token;
        }

        public string GetToken()
        {
            return token;
        }

        public void SetToken(string token)
        {
            this.token = token;
        }
    }
}
