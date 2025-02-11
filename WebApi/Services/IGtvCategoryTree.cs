using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IGtvCategoryTree
    {
        Task<IEnumerable<CategoryTreeDto>> GetAsync();
        Task<IEnumerable<CategoryTreeDto>> GetAsync(string itemCode);
    }
}