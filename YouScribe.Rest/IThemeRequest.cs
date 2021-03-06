﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest
{
    public interface IThemeRequest : IYouScribeRequest
    {
        Task<IEnumerable<ThemeModel>> GetAsync();
    }
}
