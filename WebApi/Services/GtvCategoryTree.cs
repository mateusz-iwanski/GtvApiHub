﻿using GtvApiHub.Exceptions;
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
    public class GtvCategoryTree : IGtvCategoryTree
    {
        private readonly ICategoryTreeEndPoint _services;

        public GtvCategoryTree(IGtvApiConfigurationServices services)
        {
            _services = services.CategoryTreeService;
        }

        public async Task<IEnumerable<CategoryTreeDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<CategoryTreeDto>();
            }
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"GTV API wrong response, status code: {response.StatusCode}, content: {response.Content}");

            var listCategoryTreeDto = await response.Content.GetListObjectAsync<CategoryTreeDto>();

            return listCategoryTreeDto;
        }


        public async Task<IEnumerable<CategoryTreeDto>> GetAsync(string itemCode)
        {
            var response = await _services.GetByItemCodeAsync(itemCode);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<CategoryTreeDto>();
            }
            if (!response.IsSuccessStatusCode)
                throw new ApiException($"GTV API wrong response, status code: {response.StatusCode}, content: {response.Content}");

            var listCategoryTreeDto = await response.Content.GetListObjectAsync<CategoryTreeDto>();

            return listCategoryTreeDto;
        }

    }
}
