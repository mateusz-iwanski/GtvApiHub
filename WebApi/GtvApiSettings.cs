using GtvApiHub.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi
{
    /// <summary>
    /// The class with the settings for the GtvApi.
    /// 
    /// Used to bind data from the appsettings.json file.
    /// </summary>
    public class GtvApiSettings
    {
        private string? _url;
        private string? _username;
        private string? _password;

        /// <summary>
        /// Api response file url without prefix with domain. 
        /// This is used to build the full file url.
        /// </summary>
        private string? _fileUrlPrefix;

        /// <summary>
        /// The prefix of the url for Attributes with youtube video type (AttributeType->YtUrl), 
        /// attribute from api just have the youtube video id. 
        /// </summary>
        private string? _ytUrlPrefix;

        public string? Url
        {
            get { return _url ?? throw new SettingsException($"In appsettings.json Access->GtvApi->Url not exists"); }
            set => _url = value;
        }

        public string? Username
        {
            get { return _username ?? throw new SettingsException($"In appsettings.json Access->GtvApi->Username not exists"); }
            set => _username = value;
        }

        public string? Password
        {
            get { return _password ?? throw new SettingsException($"In appsettings.json Access->GtvApi->Password not exists"); }
            set => _password = value;
        }

        public string? FileUrlPrefix
        {
            get { return _fileUrlPrefix ?? throw new SettingsException($"In appsettings.json Access->GtvApi->FileUrlPrefix not exists"); }
            set => _fileUrlPrefix = value;
        }

        public string? YtUrlPrefix
        {
            get { return _ytUrlPrefix ?? throw new SettingsException($"In appsettings.json Access->GtvApi->YtUrlPrefix not exists"); }
            set => _ytUrlPrefix = value;
        }
    }
}
