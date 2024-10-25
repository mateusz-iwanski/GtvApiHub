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
    /// <summary>
    /// Refit configuration of the specified interface.
    /// </summary>
    public interface IApiConfigurationServices
    {
        ITokenEndPoint TokenService { get; }
        IItemEndpoint ItemService { get; }
        IPriceEndPoint PriceService { get; }
        IAttributeEndPoint AttributeService { get; }
        IPackageTypeEndPoint PackageTypeService { get; }
        IAlternativeItemEndPoint AlternativeItemService { get; }
        IStockEndPoint StockService { get; }
        ICategoryTreeEndPoint CategoryTreeService { get; }
        IPromotionEndPoint PromotionService { get; }
    }

    public class ApiConfigurationServices : ApiConfiguration, IApiConfigurationServices
    {
        public ITokenEndPoint TokenService { get; private set; }
        public IItemEndpoint ItemService { get; private set; }
        public IPriceEndPoint PriceService { get; private set; }
        public IAttributeEndPoint AttributeService { get; private set; }
        public IPackageTypeEndPoint PackageTypeService { get; private set; }
        public IAlternativeItemEndPoint AlternativeItemService { get; private set; }
        public IStockEndPoint StockService { get; private set; }
        public ICategoryTreeEndPoint CategoryTreeService { get; private set; }
        public IPromotionEndPoint PromotionService { get; private set; }

        public ApiConfigurationServices(IOptions<GtvApiSettings> gtvApiSettings, IServiceProvider serviceProvider) : base(gtvApiSettings, serviceProvider)
        {
            TokenService = RestService.For<ITokenEndPoint>(httpClient);
            ItemService = RestService.For<IItemEndpoint>(httpClient);
            PriceService = RestService.For<IPriceEndPoint>(httpClient);
            AttributeService = RestService.For<IAttributeEndPoint>(httpClient);
            PackageTypeService = RestService.For<IPackageTypeEndPoint>(httpClient);
            AlternativeItemService = RestService.For<IAlternativeItemEndPoint>(httpClient);
            StockService = RestService.For<IStockEndPoint>(httpClient);
            CategoryTreeService = RestService.For<ICategoryTreeEndPoint>(httpClient);
            PromotionService = RestService.For<IPromotionEndPoint>(httpClient);
        }
    }
}
