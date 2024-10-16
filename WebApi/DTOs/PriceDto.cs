using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.DTOs
{
    public record PriceDto(
        string CardCode, 
        string ItemCode, 
        decimal BasePrice, 
        decimal FinalPrice, 
        double Discount, 
        string Currency
        ) : IBaseDto, IResponseDto;
}
