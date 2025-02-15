using GtvApiHub.Exceptions;
using GtvApiHub.WebApi.DTOs;
using GtvApiHub.WebApi.EndPointInterfaces;
using GtvApiHub.WebApi.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.Services
{
    public class GtvAlternativeItem : IGtvAlternativeItem
    {
        private readonly IAlternativeItemEndPoint _services;

        public GtvAlternativeItem(IGtvApiConfigurationServices services)
        {
            _services = services.AlternativeItemService;
        }

        public async Task<IEnumerable<AlternativeItemDto>> GetAsync()
        {
            var response = await _services.GetAsync();
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<AlternativeItemDto>();
            }
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"GTV API wrong response, status code: {response.StatusCode}, content: {response.Content}");


            var listItemDto = await response.Content.GetListObjectAsync<AlternativeItemDto>();

            return listItemDto;
        }

        public async Task<IEnumerable<AlternativeItemDto>> GetAsync(string baseItemCode)
        {
            var response = await _services.GetByBaseItemCodeAsync(baseItemCode);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<AlternativeItemDto>();
            }
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"GTV API wrong response, status code: {response.StatusCode}, content: {response.Content}");

            var listItemDto = await response.Content.GetListObjectAsync<AlternativeItemDto>();

            return listItemDto;
        }
    }
}
