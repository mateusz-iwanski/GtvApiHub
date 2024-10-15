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

namespace GtvApiHub.WebApi
{
    public class Token : IToken
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenSettingsManager _tokenSettingsManager;

        private readonly string _username;
        private readonly string _password;

        public Token(IApiConfigurationServices services, IOptions<GtvApiSettings> gtvApiSettings, ITokenSettingsManager tokenSettingsManager)
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
        public async Task<TokenResponseDto>? GetTokenAsync()
        {
            // first check if the token is not expired
            if (checkExpirationDate())
                return new TokenResponseDto
                (
                    "",
                    _tokenSettingsManager.GetTokenSettings().SecretToken,
                    DateTime.Parse(_tokenSettingsManager.GetTokenSettings().ExpiresIn)
                );

            // if expired request for a new one
            var token = await _tokenService.GetAsync(new TokenRequestDto { Username = _username, Password = _password });
            var tokenResponse = await token.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<TokenResponseDto>(tokenResponse);

            // write token to settings file
            _tokenSettingsManager.UpdateTokenSettings(deserializedResponse);

            return deserializedResponse;
        }

        private bool checkExpirationDate()
        {
            var token = _tokenSettingsManager.GetTokenSettings();
            
            if (token.ExpiresIn != null)
            {
                var newDateTime = DateTime.Now.AddMinutes(-1);
                var tokenDateTime = DateTime.Parse(token.ExpiresIn);    
                
                return newDateTime < tokenDateTime;
            }

            // token ExpiresIn is null
            return false;
        }   
    }
}
