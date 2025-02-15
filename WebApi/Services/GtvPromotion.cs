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
    public class GtvPromotion : IGtvPromotion
    {
        private readonly IPromotionEndPoint _services;

        public GtvPromotion(IGtvApiConfigurationServices services)
        {
            _services = services.PromotionService;
        }

        public async Task<IEnumerable<PromotionDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<PromotionDto>();
            }
            if (!response.IsSuccessStatusCode)
                throw new GtvApiHub.Exceptions.ApiException($"GTV API wrong response, status code: {response.StatusCode}, content: {response.Content}");

            var listItemDto = await response.Content.GetListObjectAsync<PromotionDto>();

            return listItemDto;
        }
    }
}
