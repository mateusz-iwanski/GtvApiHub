using GtvApiHub.Helpers;
using GtvApiHub.WebApi.Objects;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.EndPointInterfaces
{
    /// <summary>
    /// GtvToken interface.
    /// 
    /// Represents a token which is in the request header (OAuth 2.0).
    /// </summary>

    public interface ITokenEndPoint
    {
        [Post("/connect/token")]
        Task<HttpResponseMessage> GetAsync([Body(BodySerializationMethod.UrlEncoded)] TokenRequestDto tokenCreateDto);
    }
}
