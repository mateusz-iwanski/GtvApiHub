using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.EndPointInterfaces
{
    /// <summary>
    /// Item interface.
    /// 
    /// Represents product.
    /// </summary>
    public interface IItemEndpoint
    {
        [Get("/odata/item")]
        Task<HttpResponseMessage> GetAsync();

        [Get("/odata/item?$filter=LanguageCode eq '{code}'")]
        Task<HttpResponseMessage> GetByLanguageAsync([AliasAs("code")] string code);
    }
}
