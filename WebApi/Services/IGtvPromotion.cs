using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IGtvPromotion
    {
        Task<IEnumerable<PromotionDto>> GetAsync();
    }
}