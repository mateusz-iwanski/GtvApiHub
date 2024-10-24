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
    public class PackageType : IPackageType
    {
        private readonly IPackageTypeEndPoint _services;

        public PackageType(IApiConfigurationServices services)
        {
            _services = services.PackageTypeService;
        }

        public async Task<IEnumerable<PackageTypeDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            var listPackageTypeDto = await response.Content.GetListObjectAsync<PackageTypeDto>();

            return listPackageTypeDto;
        }

        /// <summary>
        /// Get default package type by item code
        /// </summary>
        /// <param name="itemCode">Item code</param>
        /// <returns>IEnumerable<PackageTypeDto>></returns>
        public async Task<IEnumerable<PackageTypeDto>> GetAsync(string itemCode)
        {
            var response = await _services.GetDefaultByItemCodeAsync(itemCode);

            var listPackageTypeDto = await response.Content.GetListObjectAsync<PackageTypeDto>();

            return listPackageTypeDto;
        }
    }
}
