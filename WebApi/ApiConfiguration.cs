using GtvApiHub.Exceptions;
using GtvApiHub.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi
{
    public class ApiConfiguration
    {
        protected Logger logger { get; set; }
        protected HttpClientHandler httpClientHandler { get; set; }
        protected CustomHttpClientHandler customHandler { get; set; }
        protected HttpClient httpClient { get; set; }

        private readonly IServiceProvider _serviceProvider;

        public ApiConfiguration(IOptions<GtvApiSettings> gtvApiSettings, IServiceProvider serviceProvider)
        {
            // get configuration from appsettings.json
            LogManager.Setup().LoadConfigurationFromAppSettings();
            logger = LogManager.GetCurrentClassLogger();
            httpClientHandler = new HttpClientHandler();

            _serviceProvider = serviceProvider;

            //using var scope = serviceProvider.CreateScope();
            //var token = scope.ServiceProvider.GetRequiredService<IToken>();

            customHandler = new CustomHttpClientHandler(httpClientHandler, logger, serviceProvider);

            string Uri = gtvApiSettings.Value.Url;

            httpClient = new HttpClient(customHandler)
            {
                BaseAddress = new Uri(Uri)
            };
        }
    }
}
