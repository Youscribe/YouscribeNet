using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest
{
    public interface ITokenProvider
    {
        void SetToken(string token);

        string GetToken();
    }
}
