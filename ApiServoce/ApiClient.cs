using ApiService.Resources;
using Refit;
using System;

namespace ApiService
{
    public class ApiClient
    {

        private readonly ServiceHttpClient httpClient;
        public IArticleResource Articles { get; set; }


        public ApiClient(ServiceHttpClient httpClient)
        {
            Articles = RestService.For<IArticleResource>(httpClient);

            this.httpClient = httpClient;
        }
    }
}
