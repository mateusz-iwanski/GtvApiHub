using GtvApiHub.WebApi.Objects;

namespace GtvApiHub.WebApi.Services
{
    public interface IToken
    {
        Task<TokenResponseDto> GetTokenAsync();
    }
}