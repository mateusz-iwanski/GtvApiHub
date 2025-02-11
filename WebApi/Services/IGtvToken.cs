using GtvApiHub.WebApi.Objects;

namespace GtvApiHub.WebApi.Services
{
    public interface IGtvToken
    {
        Task<TokenResponseDto> GetTokenAsync();
    }
}