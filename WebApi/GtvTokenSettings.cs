using GtvApiHub.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi
{
    /// <summary>
    /// The class with the settings for the GtvApi GtvToken.
    /// 
    /// Used to bind data from the appsettings.json file.
    /// </summary>
    public class GtvTokenSettings
    {
        private string? _secretToken { get; set; }
        private DateTime? _expiresIn { get; set; }

        public string? SecretToken
        {
            get { return _secretToken; } 
            set { _secretToken = value; }
        }

        public DateTime? ExpiresIn
        {
            get { return _expiresIn; } 
            set { _expiresIn = value; }
        }
    }
}
