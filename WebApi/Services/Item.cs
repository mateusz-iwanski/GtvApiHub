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
    public class Item : IItem
    {
        private readonly IItemEndpoint _services;

        public Item(IApiConfigurationServices services)
        {
            _services = services.ItemService;
        }

        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            var listItemDto = await response.Content.GetListObjectAsync<ItemDto>();

            return listItemDto;
        }

        public async Task<IEnumerable<ItemDto>> GetByLanguageAsync(LanguageCode languageCode)
        {
            var response = await _services.GetByLanguageAsync(languageCode.ToString());
            
            var listItemDto = await response.Content.GetListObjectAsync<ItemDto>();

            return listItemDto;
        }
    }
}
