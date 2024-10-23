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
        [Get("/odata/Stock")]
        Task<HttpResponseMessage> GetAsync();

        [Get("/odata/Stock?$filter=ItemCode eq '{ItemCode}'")]
        Task<HttpResponseMessage> GetByItemCodeAsync([AliasAs("ItemCode")] string itemCode);
    }
}
