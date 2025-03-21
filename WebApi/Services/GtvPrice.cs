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
    public class GtvPrice : IGtvPrice
    {
        private readonly IPriceEndPoint _services;

        public GtvPrice(IGtvApiConfigurationServices services)
        {
            _services = services.PriceService;
        }

        public async Task<IEnumerable<PriceDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<PriceDto>();
            }
            if (!response.IsSuccessStatusCode)
                throw new GtvApiHub.Exceptions.ApiException($"GTV API wrong response, status code: {response.StatusCode}, content: {response.Content}");

            var listItemDto = await response.Content.GetListObjectAsync<PriceDto>();

            return listItemDto;
        }

        public async Task<IEnumerable<PriceDto>> GetAsync(string itemCode)
        {
            var response = await _services.GetByItemCodeAsync(itemCode);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<PriceDto>();
            }
            if (!response.IsSuccessStatusCode)
                throw new GtvApiHub.Exceptions.ApiException($"GTV API wrong response, status code: {response.StatusCode}, content: {response.Content}");

            var listItemDto = await response.Content.GetListObjectAsync<PriceDto>();

            return listItemDto;
        }
    }
}
