using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.EndPointInterfaces
{
    public interface ICategoryTreeEndPoint
    {
        [Get("/odata/Category")]
        Task<HttpResponseMessage> GetAsync();

        [Get("/odata/Category?$filter=ItemCode eq '{ItemCode}'")]
        Task<HttpResponseMessage> GetByItemCodeAsync([AliasAs("ItemCode")] string itemCode);
    }
}
