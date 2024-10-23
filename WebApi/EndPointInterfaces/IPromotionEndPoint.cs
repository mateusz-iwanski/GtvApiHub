using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.EndPointInterfaces
{
    public interface IPromotionEndPoint
    {
        [Get("/odata/Promotion")]
        Task<HttpResponseMessage> GetAsync();
    }
}
