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
    public class Promotion : IPromotion
    {
        private readonly IPromotionEndPoint _services;

        public Promotion(IApiConfigurationServices services)
        {
            _services = services.PromotionService;
        }

        public async Task<IEnumerable<PromotionDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            var listItemDto = await response.Content.GetListObjectAsync<PromotionDto>();

            return listItemDto;
        }
    }
}
