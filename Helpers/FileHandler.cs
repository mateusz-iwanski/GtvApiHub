using Google.Cloud.Firestore;
using GtvApiHub.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Security.Authentication;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using NLog;

namespace GtvApiHub.Helpers
{
    public class HttpFileHandler
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public HttpFileHandler(ILogger logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
            return;
        }

        /// <summary>
        /// Download file from URL and save it to the specified path.
        /// If file exists, it will be overwritten.
        /// </summary>
        /// <param name="uri">URL to file to download</param>
        /// <param name="destinationPath">Path to save file</param>
        /// <exception cref="UriFormatException">Thrown when the URI is invalid</exception>
        /// <exception cref="HttpRequestException">Thrown when there is an error in the HTTP request</exception>
        /// <exception cref="TaskCanceledException">Thrown when the request times out</exception>
        /// <exception cref="Exception">Thrown when an unknown error occurs</exception>
        public async Task DownloadFileAsync(string uri, string destinationPath)
        {
            try
            {
                if (!Uri.TryCreate(uri, UriKind.Absolute, out var uriFile))
                    throw new UriFormatException("URI is invalid.");
                
                var fullPath = Path.GetFullPath(destinationPath);

                var request = new HttpRequestMessage(HttpMethod.Get, uriFile);
                request.Headers.Add("User-Agent", "HttpClient");

                using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using (var httpStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                        {
                            await httpStream.CopyToAsync(fileStream).ConfigureAwait(false);

                            _logger.Info($"File {uriFile} download completed.");
                        }
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine($"Request timed out: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }
    }
}
