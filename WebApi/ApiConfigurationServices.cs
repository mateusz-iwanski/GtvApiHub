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
        ITokenEndPoint TokenService { get; }
        IItemEndpoint ItemService { get; }
        IPriceEndPoint PriceService { get; }
        IAttributeEndPoint AttributeService { get; }
    }

    public class ApiConfigurationServices : ApiConfiguration, IApiConfigurationServices
    {
        public ITokenEndPoint TokenService { get; private set; }
        public IItemEndpoint ItemService { get; private set; }
        public IPriceEndPoint PriceService { get; private set; }
        public IAttributeEndPoint AttributeService { get; private set; }

        public ApiConfigurationServices(IOptions<GtvApiSettings> gtvApiSettings, IServiceProvider serviceProvider) : base(gtvApiSettings, serviceProvider)
        {
            TokenService = RestService.For<ITokenEndPoint>(httpClient);
            ItemService = RestService.For<IItemEndpoint>(httpClient);
            PriceService = RestService.For<IPriceEndPoint>(httpClient);
            AttributeService = RestService.For<IAttributeEndPoint>(httpClient);
        }
    }
}
