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
    public class GtvAttributeItem : IGtvAttribute
    {
        private readonly IAttributeEndPoint _services;

        public GtvAttributeItem(IGtvApiConfigurationServices services)
        {
            _services = services.AttributeService;
        }

        /// <summary>
        /// Returns all attributes  
        /// </summary>
        /// <remarks>
        /// Endpoint GetAsync response only Attribute.AttributeType == Property
        /// so, we need to get all AttributeTypes and merge them.
        /// </remarks>
        /// <returns><IEnumerable<AttributeDto></returns>
        public async Task<IEnumerable<AttributeDto>> GetAsync()
        {
            List<AttributeDto> sumOfAttributesTypes = new List<AttributeDto>();

            // get all AttributeTypes and merge them
            foreach (AttributeType type in Enum.GetValues(typeof(AttributeType)))
            {
                var byType = await GetAsync(type);  // get by AttributeType
                sumOfAttributesTypes.AddRange(byType);
            }

            return sumOfAttributesTypes;
        }

        public async Task<IEnumerable<AttributeDto>> GetAsync(string itemCode)
        {
            var response = await _services.GetByItemCodeAsync(itemCode);

            var listItemDto = await response.Content.GetListObjectAsync<AttributeDto>();

            return listItemDto;
        }

        public async Task<IEnumerable<AttributeDto>> GetAsync(LanguageCode languageCode)
        {
            var response = await _services.GetByLanguageCodeAsync(languageCode.ToString());

            var listItemDto = await response.Content.GetListObjectAsync<AttributeDto>();

            return listItemDto;
        }

        public async Task<IEnumerable<AttributeDto>> GetAsync(AttributeType attributeType)
        {
            var response = await _services.GetByAttributeTypeAsync(attributeType.ToString());

            var listItemDto = await response.Content.GetListObjectAsync<AttributeDto>();

            return listItemDto;
        }

        public async Task<IEnumerable<AttributeDto>> GetAsync(AttributeType attributeType, LanguageCode languageCode)
        {
            var response = await _services.GetByAttributeTypeLanguageCodeAsync(attributeType.ToString(), languageCode.ToString());

            var listItemDto = await response.Content.GetListObjectAsync<AttributeDto>();

            return listItemDto;
        }        
    }
}
