using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IPromotion
    {
        Task<IEnumerable<PromotionDto>> GetAsync();
    }
}