using GtvApiHub.WebApi;
using GtvApiHub.WebApi.Objects;

public interface IGtvTokenSettingsManager
{
    GtvTokenSettings GetTokenSettings();
    void UpdateTokenSettings(TokenResponseDto tokenResponseDto);
}