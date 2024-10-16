using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.EndPointInterfaces
{
    public interface IPriceEndPoint
    {
        [Get("/odata/price")]
        Task<HttpResponseMessage> GetAsync();

        [Get("/odata/Price?$filter=ItemCode eq '{ItemCode}'")]
        Task<HttpResponseMessage> GetByItemCodeAsync([AliasAs("ItemCode")] string ItemCode);
    }
}
