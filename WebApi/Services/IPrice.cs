using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IPrice
    {
        Task<IEnumerable<PriceDto>> GetAsync();
        Task<IEnumerable<PriceDto>> GetByItemCodeAsync(string itemCode);
    }
}