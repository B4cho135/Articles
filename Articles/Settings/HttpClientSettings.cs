using ApiTools;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Settings
{
    public class HttpClientSettings : IHttpClientSettings
    {
        private readonly IConfiguration _configuration;
        public HttpClientSettings(IConfiguration configuration)
        {
            _configuration = configuration;
           
        }

        public string ServiceUrl => _configuration["ApiBaseURL"];

    }
}
