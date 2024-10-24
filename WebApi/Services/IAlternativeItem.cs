using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IAlternativeItem
    {
        Task<IEnumerable<AlternativeItemDto>> GetAsync();
        Task<IEnumerable<AlternativeItemDto>> GetAsync(string baseItemCode);
    }
}