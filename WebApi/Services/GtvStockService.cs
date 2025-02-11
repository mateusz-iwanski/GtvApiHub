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
    public class GtvStockService : IGtvStockService
    {
        private readonly IStockEndPoint _services;

        public GtvStockService(IGtvApiConfigurationServices services)
        {
            _services = services.StockService;
        }

        public async Task<IEnumerable<StockDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            var listStockDto = await response.Content.GetListObjectAsync<StockDto>();

            return listStockDto;
        }

        public async Task<IEnumerable<StockDto>> GetAsync(string itemCode)
        {
            var response = await _services.GetByItemCodeAsync(itemCode);

            var listStockDto = await response.Content.GetListObjectAsync<StockDto>();

            return listStockDto;
        }
    }
}
