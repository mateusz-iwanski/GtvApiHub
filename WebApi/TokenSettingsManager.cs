﻿using System.IO;
using GtvApiHub.Exceptions;
using GtvApiHub.WebApi;
using GtvApiHub.WebApi.Objects;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

/// <summary>
/// Token settings manager.
/// 
/// Class responsible for save and get token settings from appsettings.json.
/// </summary>
/// <remarks>
/// In appsettings.json you must have a TokenSettings section.  
/// 
/// "TokenSettings": {
///     "SecretToken": "",
///     "ExpiresIn": ""
/// }
/// </remarks>
public class TokenSettingsManager : ITokenSettingsManager
{
    private readonly string _filePath = "appsettings.json";

    // Monitor for token settings. File can change and this interface will be notified.
    private readonly IOptionsMonitor<TokenSettings> _tokenSettingsMonitor;

    public TokenSettingsManager(IOptionsMonitor<TokenSettings> tokenSettingsMonitor)
    {
        _tokenSettingsMonitor = tokenSettingsMonitor;
    }

    /// <summary>
    /// Update appsettings file with properly access token
    /// </summary>
    /// <param name="tokenResponseDto">TokenResponseDto</param>
    public void UpdateTokenSettings(TokenResponseDto tokenResponseDto)
    {
        var json = File.ReadAllText(_filePath);
        var jsonObj = JObject.Parse(json);

        var tokenSettings = jsonObj["TokenSettings"] ?? 
            throw new SettingsException("The TokenSettings section must exist in the appsettings.json file");

        tokenSettings["SecretToken"] = tokenResponseDto.AccessToken;
        tokenSettings["ExpiresIn"] = tokenResponseDto.ExpiresIn;
            
        File.WriteAllText(_filePath, jsonObj.ToString());
    }

    /// <summary>
    /// Get token settings from appsettings.json
    /// </summary>
    /// <returns>TokenSettings</returns>
    public TokenSettings GetTokenSettings()
    {
        return new TokenSettings
        {
            SecretToken = _tokenSettingsMonitor.CurrentValue.SecretToken,
            ExpiresIn = _tokenSettingsMonitor.CurrentValue.ExpiresIn
        };
    }
}
