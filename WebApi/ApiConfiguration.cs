using GtvApiHub.Exceptions;
using GtvApiHub.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi
{
    public class ApiConfiguration
    {
        protected Logger logger { get; set; }
        protected HttpClientHandler httpClientHandler { get; set; }
        protected CustomHttpClientHandler customHandler { get; set; }
        protected HttpClient httpClient { get; set; }

        public ApiConfiguration(IOptions<GtvApiSettings> gtvApiSettings)
        {
            // get configuration from appsettings.json
            LogManager.Setup().LoadConfigurationFromAppSettings();
            logger = LogManager.GetCurrentClassLogger();
            httpClientHandler = new HttpClientHandler();
            customHandler = new CustomHttpClientHandler(httpClientHandler, logger);

            string Uri = gtvApiSettings.Value.Url;

            httpClient = new HttpClient(customHandler)
            {
                BaseAddress = new Uri(Uri)
            };
        }
    }
}
