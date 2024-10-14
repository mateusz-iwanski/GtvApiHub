using GtvApiHub.WebApi;
using GtvApiHub.WebApi.Objects;

public interface ITokenSettingsManager
{
    TokenSettings GetTokenSettings();
    void UpdateTokenSettings(TokenResponseDto tokenResponseDto);
}