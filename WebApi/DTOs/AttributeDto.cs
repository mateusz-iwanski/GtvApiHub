using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.DTOs
{
    public record AttributeDto(
        string AttributeName, 
        string AttributeType, 
        string ItemCode, 
        string LanguageCode, 
        string Value, 
        string FileHandler
        ) : IBaseDto, IResponseDto;
}
