using System.IO;
using FirebaseManager.Firebase;
using FirebaseManager.Firestore;
using FirebaseManager.Storage;
using Google.Cloud.Firestore;
using GtvApiHub.Helpers;
using GtvApiHub.WebApi.DTOs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace GtvApiHub.WebApi.Services
{
    /// <summary>
    /// FirestorageDtoFileHandler copy file from url to local directory and upload to Firebase Storage.
    /// </summary>
    public class FirestorageDtoFileHandler : IFirestorageFileHandler
    {
        private readonly IFirestorageService _firestorageService;
        private readonly IOptions<FirebaseSettings> _firebaseSettings;
        private readonly IOptions<GtvApiSettings> _gtvApiSettings;
        private readonly ILogger _logger;

        public FirestorageDtoFileHandler(
            IFirestorageService firestorageService, 
            IOptions<FirebaseSettings> settings, 
            IOptions<GtvApiSettings> gtvApiSettings,
            ILogger logger)
        {
            _firestorageService = firestorageService;
            _firebaseSettings = settings;
            _gtvApiSettings = gtvApiSettings;
            _logger = logger;
        }

        /// <summary>
        /// Download and upload file to Firebase Storage from DTO.
        /// </summary>
        /// <param name="dto">IStorageStrategy</param>
        /// <returns>If DTO doesn't have file return false, true if store correctly</returns>
        public async Task<bool> StoreAsync(IStorageStrategy dto)
        {
            var urlFile = dto.GetFilePath();
            if (urlFile == null)
                return false;

            urlFile = Path.Combine(
                _gtvApiSettings.Value.FileUrlPrefix,                
                dto.GetFilePath()
                );            

            // Check if the directory exists, if not, create it
            string directoryPath = _firebaseSettings.Value.LocalDirectoryForfileToDownload;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = UrlHelper.GetFileNameFromUrl(urlFile);

            string filePathToSave = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                _firebaseSettings.Value.LocalDirectoryForfileToDownload,
                fileName
                );

            await new HttpFileHandler(_logger).DownloadFileAsync(urlFile, filePathToSave);

            await _firestorageService.UploadFileAsync(filePathToSave, fileName);

            return true;
        }

    }
}
