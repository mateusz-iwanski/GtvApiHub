using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IGtvPrice
    {
        Task<IEnumerable<PriceDto>> GetAsync();
        Task<IEnumerable<PriceDto>> GetAsync(string itemCode);
    }
}