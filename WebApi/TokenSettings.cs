using GtvApiHub.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi
{
    public class TokenSettings
    {
        private string? _secretToken { get; set; }
        private DateTime? _expiresIn { get; set; }

        public string? SecretToken
        {
            get { return _secretToken; } ///?? throw new SettingsException($"In appsettings.json TokenSettings->SecretToken not exists"); }
            set { _secretToken = value; }
        }

        public DateTime? ExpiresIn
        {
            get { return _expiresIn; } //?? throw new SettingsException($"In appsettings.json TokenSettings->ExpiresIn not exists"); }
            set { _expiresIn = value; }
        }
    }
}
