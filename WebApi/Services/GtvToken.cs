using GtvApiHub.Exceptions;
using GtvApiHub.WebApi.EndPointInterfaces;
using GtvApiHub.WebApi.Objects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.Services
{
    /// <summary>
    /// GtvToken class is responsible for getting the token from the API.
    /// 
    /// The token has an expiration date. If the token has expired, 
    /// a new one is requested and saved in the application settings, 
    /// if not expired, token is returned from settings.
    /// If the token is not in the settings, a new one is requested.
    /// </summary>
    public class GtvToken : IGtvToken
    {
        private readonly ITokenEndPoint _tokenService;
        private readonly IGtvTokenSettingsManager _tokenSettingsManager;

        private readonly string _username;
        private readonly string _password;

        public GtvToken(IGtvApiConfigurationServices services, IOptions<GtvApiSettings> gtvApiSettings, IGtvTokenSettingsManager tokenSettingsManager)
        {
            _tokenService = services.TokenService;
            _tokenSettingsManager = tokenSettingsManager;

            _username = gtvApiSettings.Value.Username;
            _password = gtvApiSettings.Value.Password;
        }

        /// <summary>
        /// Get token from the API.
        /// 
        /// The token has an expiration date.
        /// </summary>
        /// <returns>TokenResponseDto or null if this is a problem</returns>
        [DeserializeWebApiResponse]
        public async Task<TokenResponseDto> GetTokenAsync()
        {
            // first check if the token is not expired
            if (checkExpirationDate())
                return new TokenResponseDto
                (
                    "",
                    _tokenSettingsManager.GetTokenSettings().SecretToken,
                    _tokenSettingsManager.GetTokenSettings().ExpiresIn ?? DateTime.MinValue
                );

            // if expired request for a new one
            var token = await _tokenService.GetAsync(new TokenRequestDto { Username = _username, Password = _password });
            var tokenResponse = await token.Content.ReadAsStringAsync();

            var deserializedResponse = JsonConvert.DeserializeObject<TokenResponseDto>(tokenResponse) ??
                throw new SettingsException("Access token failure.");

            // write token to settings file
            _tokenSettingsManager.UpdateTokenSettings(deserializedResponse);

            return deserializedResponse;
        }

        /// <summary>
        /// Check if the token has not expired.
        /// </summary>
        /// <returns></returns>
        private bool checkExpirationDate()
        {
            var token = _tokenSettingsManager.GetTokenSettings();

            if (token.ExpiresIn != null)
            {
                var newDateTime = DateTime.Now.AddMinutes(-1);
                var tokenDateTime = token.ExpiresIn;

                return newDateTime < tokenDateTime;
            }

            // token ExpiresIn is null
            return false;
        }
    }
}
