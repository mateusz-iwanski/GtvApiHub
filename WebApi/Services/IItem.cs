using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IItem
    {
        Task<IEnumerable<ItemDto>> GetAsync();
        Task<IEnumerable<ItemDto>> GetByLanguageAsync(LanguageCode languageCode);
    }
}