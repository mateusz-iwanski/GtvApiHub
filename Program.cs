
//namespace GtvApiHub
//{
//    internal class Program
//    {
//        public static void Main(string[] args)
//        {
//        }
//    }
//}

using FirebaseManager.Firestore;
using FirebaseManager.Storage;
using GtvApiHub.Helpers;
using GtvApiHub.WebApi;
using GtvApiHub.WebApi.DTOs;
using GtvApiHub.WebApi.FirebaseManagement;
using GtvApiHub.WebApi.Objects;
using GtvApiHub.WebApi.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;


namespace GtvApiHub
{
    internal class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var serviceProvider = host.Services;

            Console.WriteLine("Hello, World!");

            using var scope = serviceProvider.CreateScope();

            var token = scope.ServiceProvider.GetRequiredService<IGtvToken>();
            await token.GetTokenAsync();

            // ITEM SERVICE
            //var itemService = scope.ServiceProvider.GetRequiredService<IGtvItem>();
            //var service = await itemService.GetAsync();
            //var service = await itemService.GetAsync(LanguageCode.pl);

            // PIECE SERVICE
            //var priceService = scope.ServiceProvider.GetRequiredService<IGtvPrice>();
            //var service = await priceService.GetAsync();
            //var service = await priceService.GetAsync("ZZ-ZN-338-01-S");

            // ATTRIBUTE SERVICE
            //var attributeService = scope.ServiceProvider.GetRequiredService<IGtvAttribute>();
            //var service = await attributeService.GetAsync();
            //var service = await attributeService.GetAsync("ZZ-ZN-338-01-S");
            //var service = await attributeService.GetAsync(LanguageCode.uk);
            //var service = await attributeService.GetAsync(AttributeType.File, LanguageCode.pl);

            // PACKAGETYPE SERVICE
            //var packageTypeService = scope.ServiceProvider.GetRequiredService<IGtvPackageType>();
            //var service = await packageTypeService.GetAsync();
            //var service = await packageTypeService.GetAsync("ZZ-ZN-338-01-S");

            // ALTERNATIVE ITEM SERVICE
            var alternativeItemService = scope.ServiceProvider.GetRequiredService<IGtvAlternativeItem>();
            var service = await alternativeItemService.GetAsync();
            // service = await alternativeItemService.GetAsync("ZZ-Z0-202-01");

            // STOCK SERVICE
            //var stockService = scope.ServiceProvider.GetRequiredService<IGtvStockService>();
            //var service = await stockService.GetAsync();
            //var service = await stockService.GetAsync("00-KLUCZREG-DPADPD");

            // CATEGORY TREE SERVICE
            //var categoryTreeService = scope.ServiceProvider.GetRequiredService<IGtvCategoryTree>();
            //var service = await categoryTreeService.GetAsync();
            //var service = await categoryTreeService.GetAsync("ZZ-ZN-338-01-S");

            // PROMOTION SERVICE
            //var promotionService = scope.ServiceProvider.GetRequiredService<IGtvPromotion>();
            //var service = await promotionService.GetAsync();

            foreach (var i in service)
            {
                Console.WriteLine("---> " + i);
            }

            // UPLOAD FILE TO FIRESTORAGE
            //var fsDtoFileHandler = scope.ServiceProvider.GetRequiredService<IGtvFirestorageFileHandler>();
            //var s = await attributeService.GetAsync(AttributeType.File, LanguageCode.pl); // Get the data from the API
            //var k = s.ToList()[0]; // get just one item
            //await fsDtoFileHandler.StoreAsync(k);


            // INSERT SUB-COLLECTION INTO NEW COLLECTION
            //var i = new ItemDto { ItemCode = "ItemCodeTest", ItemName = "ItemNameTest", LanguageCode = "PL" };
            //var attr = new AttributeDto { ItemCode = "ItemCodeTest", AttributeType = "File", LanguageCode = "PL", Value = "ValueTest", AttributeName = "AttributeNameXX" };
            //var fs = scope.ServiceProvider.GetRequiredService<IFirestoreService>();
            //await fs.InsertDtoAsync(i);
            //await fs.InsertDtoWithSubDtoAsync(i, attr);

            // CREATE CONTENTS


            // SYNCING
            //var itemManager = scope.ServiceProvider.GetRequiredService<GtvFirestoreSyncItemManager>();
            //await itemManager.AddNewOneTemp();
            //await itemManager.AddNewOne(continueOnError: true);
            //await itemManager.SyncFirestoreAsync();  // SYNC VRYTHING
            //await itemManager.SyncPrice();
            //await itemManager.SyncStock();
            //await itemManager.SyncAttribute(continueOnError: true);
            //await itemManager.SyncPackageType();
            //await itemManager.SyncItem();
            //await itemManager.SyncAlternativeItem();
            //await itemManager.SyncId(continueOnError: false);

            //await itemManager.CreateContents(new List<FirestoreItemDto>());


            return 0; // Ensure a value is returned
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
               .ConfigureServices((context, services) =>
               {
                   var startup = new Startup();
                   startup.ConfigureServices(context, services);
               });
    }
}
