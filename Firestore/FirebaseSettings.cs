using GtvApiHub.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.Firestore
{
    /// <summary>
    /// The class with the settings for the Firebase service.
    /// 
    /// Used to bind data from the appsettings.json file.
    /// </summary>
    public class FirebaseSettings
    {
        private string? _projectId;
        private string? _apiKeyFilePath;

        public string? ProjectId
        {
            get { return _projectId ?? throw new SettingsException($"In appsettings.json Firebase->Firestore->ProjectId not exists"); }
            set => _projectId = value;
        }

        public string? ApiKeyFilePath
        {
            get { return _apiKeyFilePath ?? throw new SettingsException($"In appsettings.json Firebase->ApiKeyFilePath not exists"); }
            set => _apiKeyFilePath = value;
        }
    }
}
