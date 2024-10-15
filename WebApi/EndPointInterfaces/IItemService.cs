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
    public interface IItemService
    {
        [Get("/odata/item")]
        Task<HttpResponseMessage> GetAsync();
    }
}
