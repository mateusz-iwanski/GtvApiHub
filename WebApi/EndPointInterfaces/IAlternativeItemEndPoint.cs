using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.EndPointInterfaces
{
    public interface IAlternativeItemEndPoint
    {
        [Get("/odata/Alternativeitem")]
        Task<HttpResponseMessage> GetAsync();

        [Get("/odata/Alternativeitem?$filter=BaseItemCode eq '{ItemCode}'")]
        Task<HttpResponseMessage> GetByBaseItemCodeAsync([AliasAs("ItemCode")] string itemCode);
    }
}
