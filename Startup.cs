using FirebaseManager.Firebase;
using FirebaseManager.Firestore;
using FirebaseManager.Storage;
using GtvApiHub.WebApi;
using GtvApiHub.WebApi.FirebaseManagement;
using GtvApiHub.WebApi.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace GtvApiHub
{
    /// <summary>
    /// Represents a builder for configuring services.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the ConfigureServices class.
        /// </summary>
        public void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {

            services.AddSingleton<IConfiguration>((ConfigurationBuilder) =>
            {
                return new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            });

            // Add NLog as the logging provider
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders(); // Clear any existing logging providers
                loggingBuilder.SetMinimumLevel(LogLevel.Trace); // Set the minimum log level

                // Add NLog
                loggingBuilder.AddNLog();
            });
            // Register NLog's ILogger
            services.AddSingleton<NLog.ILogger>(provider => NLog.LogManager.GetCurrentClassLogger());

            // Add options to bind to the configuration instance
            services.Configure<GtvApiSettings>(context.Configuration.GetSection("GtvApiAccess").GetSection("GtvApi"));
            services.Configure<GtvTokenSettings>(context.Configuration.GetSection("GtvApiTokenSettings"));
            services.Configure<FirebaseSettings>(context.Configuration.GetSection("Firebase"));

            // Add Firestore services
            services.AddScoped<IFirestoreConnector, FirestoreConnector>(); 
            services.AddScoped<IFirestoreService, FirestoreService>();
            services.AddScoped<IFirestorageConnector, FirestorageConnector>();
            services.AddScoped<IFirestorageService, FirestorageService>();
            

            // Add GTV Api services                                    
            services.AddScoped<IGtvApiConfigurationServices, GtvApiConfigurationServices>();            
            services.AddScoped<IGtvTokenSettingsManager, GtvTokenSettingsManager>();
            services.AddScoped<IGtvFirestorageFileHandler, GtvFirestorageDtoFileHandler>();

            services.AddScoped<IGtvItem, GtvItem>();
            services.AddScoped<IGtvToken, GtvToken>();
            services.AddScoped<IGtvPrice, GtvPrice>();
            services.AddScoped<IGtvAttribute, GtvAttributeItem>();
            services.AddScoped<IGtvPackageType, GtvPackageType>();
            services.AddScoped<IGtvAlternativeItem, GtvAlternativeItem>();
            services.AddScoped<IGtvStockService, GtvStockService>();
            services.AddScoped<IGtvCategoryTree, GtvCategoryTree>();
            services.AddScoped<IGtvPromotion, GtvPromotion>();

            services.AddScoped<GtvFirestoreSyncItemManager>();
        }
    }
}
