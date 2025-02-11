using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.EndPointInterfaces
{
    public interface IStockEndPoint
    {
        [Get("/odata/StockService")]
        Task<HttpResponseMessage> GetAsync();

        [Get("/odata/StockService?$filter=ItemCode eq '{ItemCode}'")]
        Task<HttpResponseMessage> GetByItemCodeAsync([AliasAs("ItemCode")] string itemCode);
    }
}
