using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.EndPointInterfaces
{
    public interface IPackageTypeEndPoint
    {
        [Get("/odata/Unit")]
        Task<HttpResponseMessage> GetAsync();

        [Get("/odata/Unit?$filter=ItemCode eq '{ItemCode}' and isDefaultUnit eq true")]
        Task<HttpResponseMessage> GetDefaultByItemCodeAsync([AliasAs("ItemCode")] string itemCode);
    }
}
