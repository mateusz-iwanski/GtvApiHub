using GtvApiHub.WebApi.DTOs;
using GtvApiHub.WebApi.EndPointInterfaces;
using GtvApiHub.WebApi.Extensions;
using GtvApiHub.WebApi.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.Services
{
    /// <summary>
    /// Represents a collection of elements in the GtvApi service
    /// </summary>
    public class GtvItem : IGtvItem
    {
        private readonly IItemEndpoint _services;

        public GtvItem(IGtvApiConfigurationServices services)
        {
            _services = services.ItemService;
        }

        /// <summary>
        /// Get all items from api.
        /// 
        /// Default it will return only with LanguageCode.pl
        /// </summary>
        /// <returns>IEnumerable<ItemDto></returns>
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            var listItemDto = await response.Content.GetListObjectAsync<ItemDto>();

            return listItemDto;
        }

        /// <summary>
        /// Get all items in the selected language
        /// </summary>
        /// <param name="languageCode">LanguageCode </param>
        /// <returns>IEnumerable<ItemDto</returns>
        public async Task<IEnumerable<ItemDto>> GetAsync(LanguageCode languageCode)
        {
            var response = await _services.GetByLanguageAsync(languageCode.ToString());
            
            var listItemDto = await response.Content.GetListObjectAsync<ItemDto>();

            return listItemDto;
        }
    }
}
