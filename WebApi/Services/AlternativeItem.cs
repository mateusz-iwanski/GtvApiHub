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
    public class AlternativeItem : IAlternativeItem
    {
        private readonly IAlternativeItemEndPoint _services;

        public AlternativeItem(IApiConfigurationServices services)
        {
            _services = services.AlternativeItemService;
        }

        public async Task<IEnumerable<AlternativeItemDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            var listItemDto = await response.Content.GetListObjectAsync<AlternativeItemDto>();

            return listItemDto;
        }

        public async Task<IEnumerable<AlternativeItemDto>> GetAsync(string baseItemCode)
        {
            var response = await _services.GetByBaseItemCodeAsync(baseItemCode);

            var listItemDto = await response.Content.GetListObjectAsync<AlternativeItemDto>();

            return listItemDto;
        }
    }
}
