using GtvApiHub.WebApi.EndPointInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi
{
    public interface IApiConfigurationServices
    {
        ITokenService TokenService { get; }
    }

    public class ApiConfigurationServices : ApiConfiguration, IApiConfigurationServices
    {
        public ITokenService TokenService { get; private set; }

        public ApiConfigurationServices(IOptions<GtvApiSettings> gtvApiSettings) : base(gtvApiSettings)
        {
            TokenService = RestService.For<ITokenService>(httpClient);
        }
    }
}
