﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YouScribe.Rest.Models.Products;

namespace YouScribe.Rest
{
    class ProductCommentRequest : YouScribeRequest, IProductCommentRequest
    {
        public ProductCommentRequest(Func<DisposableClient> clientFactory, ITokenProvider authorizeTokenProvider)
            : base(clientFactory, authorizeTokenProvider)
        { }


        public async Task<ProductCommentsOutput> GetCommentsAsync(int productId, int skip = 0, int take = 5, int repliesTake = 3)
        {
            var url = "api/v1/products/" + productId + "/comments";
            var dico = new Dictionary<string, string>(){
                { "skip", skip.ToString() },
                { "take", take.ToString() },
                { "repliesTake", repliesTake.ToString() }
            };
            url = url + "?" + dico.ToQueryString();
            var model = await this.GetAsync<ProductCommentsOutput>(url);
            if (model == null)
                return new ProductCommentsOutput() { Count = -1, Comments = Enumerable.Empty<ProductCommentOutput>() };
            return model;
        }
    }
}
