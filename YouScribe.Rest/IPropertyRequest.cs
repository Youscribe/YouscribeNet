using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest
{
    public interface IPropertyRequest : IYouScribeRequest
    {
        Task<IEnumerable<PropertyModel>> GetAsync(string type);
    }
}
