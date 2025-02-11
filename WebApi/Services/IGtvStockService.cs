using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IGtvStockService
    {
        Task<IEnumerable<StockDto>> GetAsync();
        Task<IEnumerable<StockDto>> GetAsync(string itemCode);
    }
}