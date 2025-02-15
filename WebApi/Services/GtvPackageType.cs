using GtvApiHub.WebApi.DTOs;
using GtvApiHub.WebApi.EndPointInterfaces;
using GtvApiHub.WebApi.Extensions;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.Services
{
    public class GtvPackageType : IGtvPackageType
    {
        private readonly IPackageTypeEndPoint _services;

        public GtvPackageType(IGtvApiConfigurationServices services)
        {
            _services = services.PackageTypeService;
        }

        public async Task<IEnumerable<PackageTypeDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<PackageTypeDto>();
            }
            if (!response.IsSuccessStatusCode)
                throw new GtvApiHub.Exceptions.ApiException($"GTV API wrong response, status code: {response.StatusCode}, content: {response.Content}");

            var listPackageTypeDto = await response.Content.GetListObjectAsync<PackageTypeDto>();

            return listPackageTypeDto;
        }

        /// <summary>
        /// Get default package type by item code
        /// </summary>
        /// <param name="itemCode">GtvItem code</param>
        /// <returns>IEnumerable<PackageTypeDto>></returns>
        public async Task<IEnumerable<PackageTypeDto>> GetAsync(string itemCode)
        {
            var response = await _services.GetDefaultByItemCodeAsync(itemCode);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<PackageTypeDto>();
            }
            if (!response.IsSuccessStatusCode)
                throw new GtvApiHub.Exceptions.ApiException($"GTV API wrong response, status code: {response.StatusCode}, content: {response.Content}");

            var listPackageTypeDto = await response.Content.GetListObjectAsync<PackageTypeDto>();

            return listPackageTypeDto;
        }
    }
}
