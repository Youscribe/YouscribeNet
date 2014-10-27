using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest
{
    public interface ISerializer
    {
        T Deserialize<T>(string data);

        string Serialize<T>(T obj);
    }
}
