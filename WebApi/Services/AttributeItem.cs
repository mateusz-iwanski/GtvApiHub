using GtvApiHub.WebApi.DTOs;
using GtvApiHub.WebApi.EndPointInterfaces;
using GtvApiHub.WebApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.Services
{
    public class AttributeItem : IAttribute
    {
        private readonly IAttributeEndPoint _services;

        public AttributeItem(IApiConfigurationServices services)
        {
            _services = services.AttributeService;
        }

        public async Task<IEnumerable<AttributeDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            var listItemDto = await response.Content.GetListObjectAsync<AttributeDto>();

            return listItemDto;
        }

        public async Task<IEnumerable<AttributeDto>> GetByItemCodeAsync(string itemCode)
        {
            var response = await _services.GetByItemCodeAsync(itemCode);

            var listItemDto = await response.Content.GetListObjectAsync<AttributeDto>();

            return listItemDto;
        }

        public async Task<IEnumerable<AttributeDto>> GetByLanguageCodeAsync(LanguageCode languageCode)
        {
            var response = await _services.GetByLanguageCodeAsync(languageCode.ToString());

            var listItemDto = await response.Content.GetListObjectAsync<AttributeDto>();

            return listItemDto;
        }

        public async Task<IEnumerable<AttributeDto>> GetByAttributeTypeAsync(AttributeType attributeType)
        {
            var response = await _services.GetByAttributeTypeAsync(attributeType.ToString());

            var listItemDto = await response.Content.GetListObjectAsync<AttributeDto>();

            return listItemDto;
        }

        public async Task<IEnumerable<AttributeDto>> GetByAttributeTypeAndLanguageCodeAsync(AttributeType attributeType, LanguageCode languageCode)
        {
            var response = await _services.GetByAttributeTypeAndLanguageCodeAsync(attributeType.ToString(), languageCode.ToString());

            var listItemDto = await response.Content.GetListObjectAsync<AttributeDto>();

            return listItemDto;
        }
    }
}
