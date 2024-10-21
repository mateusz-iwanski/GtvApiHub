﻿using GtvApiHub.WebApi.DTOs;
using GtvApiHub.WebApi.EndPointInterfaces;
using GtvApiHub.WebApi.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.Services
{
    /// <summary>
    /// Represents a price collection in the GtvApi service
    /// </summary>
    public class Price : IPrice
    {
        private readonly IPriceEndPoint _services;

        public Price(IApiConfigurationServices services)
        {
            _services = services.PriceService;
        }

        public async Task<IEnumerable<PriceDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            var listItemDto = await response.Content.GetListObjectAsync<PriceDto>();

            return listItemDto;
        }

        public async Task<IEnumerable<PriceDto>> GetByItemCodeAsync(string itemCode)
        {
            var response = await _services.GetByItemCodeAsync(itemCode);

            var listItemDto = await response.Content.GetListObjectAsync<PriceDto>();

            return listItemDto;
        }
    }
}