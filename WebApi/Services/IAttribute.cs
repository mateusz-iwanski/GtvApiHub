using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IAttribute
    {
        Task<IEnumerable<AttributeDto>> GetAsync();
        Task<IEnumerable<AttributeDto>> GetByItemCodeAsync(string itemCode);
        Task<IEnumerable<AttributeDto>> GetByLanguageCodeAsync(LanguageCode languageCode);
    }
}