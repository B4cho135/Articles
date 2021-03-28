using ApiTools;
using System;
using System.Net.Http;

namespace ApiService
{
    public class ServiceHttpClient : HttpClient
    {
        public ServiceHttpClient(IHttpClientSettings settings)
        {
            this.BaseAddress = new Uri(settings.ServiceUrl);
        }
    }
}
