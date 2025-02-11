using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IGtvAttribute
    {
        Task<IEnumerable<AttributeDto>> GetAsync();
        Task<IEnumerable<AttributeDto>> GetAsync(string itemCode);
        Task<IEnumerable<AttributeDto>> GetAsync(LanguageCode languageCode);
        Task<IEnumerable<AttributeDto>> GetAsync(AttributeType attributeType);
        Task<IEnumerable<AttributeDto>> GetAsync(AttributeType attributeType, LanguageCode languageCode);
    }
}