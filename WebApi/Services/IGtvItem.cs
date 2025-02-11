using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IGtvItem
    {
        Task<IEnumerable<ItemDto>> GetAsync();
        Task<IEnumerable<ItemDto>> GetAsync(LanguageCode languageCode);
    }
}