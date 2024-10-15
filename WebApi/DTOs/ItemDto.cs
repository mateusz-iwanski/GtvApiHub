using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.DTOs
{
    /// <summary>
    /// Item DTO represents products from Api.
    /// </summary>
    /// <param name="ItemCode">Symbol</param>
    /// <param name="ItemName">Ttile</param>
    /// <param name="LanguageCode">Language code</param>
    public record ItemDto(string ItemCode, string ItemName, string LanguageCode) : IBaseDto, IResponseDto;
}
