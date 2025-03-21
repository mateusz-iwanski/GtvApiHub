﻿using FirebaseManager.Exceptions;
using FirebaseManager.Firebase;
using FirebaseManager.Firestore;
using GtvApiHub.WebApi.DTOs;
using GtvApiHub.WebApi.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using NLog;
using System.Linq;
using System.Reflection;

namespace GtvApiHub.WebApi.FirebaseManagement
{
    /// <summary>
    /// GtvFirestoreSyncItemManager is responsible for syncing items between GTV API and Firestore
    /// 
    /// Firestore Items collection has all data as GtvPrice, Stocks, Attributes etc. in one document.
    /// Syncing object is FirestoreItemDto, by this object schema it's save in Firestore
    /// </summary>
    public class GtvFirestoreSyncItemManager : FirestoreSyncManager, IFirestoreManager
    {
        private List<FirestoreItemDto> _apiItems { get; set; }
        private List<FirestoreItemDto> _firestoreItems { get; set; }

        private readonly IGtvItem _itemService;
        private readonly IGtvPrice _priceService;
        private readonly IGtvStockService _stockService;
        private readonly IGtvAttribute _attributeService;
        private readonly IGtvCategoryTree _categoryTreeService;
        private readonly IGtvPackageType _packageTypeService;
        private readonly IGtvAlternativeItem _alternativeItem;
        private readonly IGtvFirestorageFileHandler _firestorageFileHandler;
        private readonly IOptions<GtvApiSettings> _gtvApiSettings;
        private readonly IOptions<FirebaseSettings> _firebaseApiSettings;

        /// <summary>
        /// Use if you want to sync only item DTOs without rest data (attributes, price, stocks....)
        /// </summary>
        /// <param name="item"></param>
        /// <param name="firestoreService"></param>
        /// <param name="logger"></param>
        public GtvFirestoreSyncItemManager(
            IGtvItem itemService,
            IGtvPrice priceService,
            IGtvStockService stockService,
            IGtvAttribute attributeService,
            IGtvCategoryTree categoryTreeService,
            IGtvPackageType packageTypeService,
            IFirestoreService firestoreService,
            IGtvAlternativeItem alternativeItem,
            IGtvFirestorageFileHandler firestorageFileHandler,
            IOptions<GtvApiSettings> gtvApiSettings,
            IOptions<FirebaseSettings> firebaseApiSettings,
            ILogger logger)
            : base(firestoreService, logger)
        {
            _itemService = itemService;
            _priceService = priceService;
            _stockService = stockService;
            _attributeService = attributeService;
            _categoryTreeService = categoryTreeService;
            _packageTypeService = packageTypeService;
            _alternativeItem = alternativeItem;
            _firestorageFileHandler = firestorageFileHandler;
            _gtvApiSettings = gtvApiSettings;
            _firebaseApiSettings = firebaseApiSettings;
        }

        /// <summary>
        /// Fill in data from API and Firestore
        /// </summary>        
        private async Task FillInDataAsync()
        {
            if (_apiItems == null || _firestoreItems == null)
            {
                _apiItems = await getAllFromApiAsync();
                //_firestoreItems = await GetAllFromFirestoreAsync(_apiItems);
                _firestoreItems = await GetAllFirestoreDocumentsInCollectionAsync<FirestoreItemDto>("Gtv_Items");
            }
        }

        /// <summary>
        /// Create contsnts for storing the id documents with the item code inside documents.
        /// </summary>
        /// <returns></returns>
        public async Task CreateContents(List<FirestoreItemDto> items)
        {
            await FillInDataAsync();
            //if (_firestoreItems == null)
            //    _firestoreItems = await GetAllFromFirestoreAsync(_apiItems);
            //_firestoreItems = await GetAllFirestoreDocumentsInCollectionAsync<FirestoreItemDto>("Gtv_Items");
            var Content = new FirestoreContentsDtoBuilder().Build(items);
            // first delete
            await FirestoreService.DeleteDtoAsync(Content);
            // add new one
            await FirestoreService.InsertDtoAsync(Content);
        }

        public async Task AddNewOneTemp()
        {
            await FillInDataAsync();

            var itemsWithFileHandler = _apiItems.Where(item => item.Attributes != null);

            //foreach (var a in itemsWithFileHandler)
            //    if (a.Attributes.Any(attr => attr.AttributeType == "File")) Console.WriteLine(a + "-----" + a.Attributes.Any(attr => attr.AttributeType == "File"));

            var k = itemsWithFileHandler.Where(x => x.Attributes.Any(attr => attr.AttributeType == AttributeType.File.ToString())).ToList();

            for (int i = 0; i < 5; i++)
                await AddDtoFirestore(k[i], _firestoreItems);
            
        }

        /// <summary>
        /// Add new documents to Firestore if doesn't exist
        /// </summary>
        /// <param name="continueOnError">If this is true, don't stop if some records contain exceptions. Default is false</param>
        /// <returns></returns>
        /// <exception cref="FirestoreException">Raise when is a problem with adding object to Firestore</exception>
        public async Task AddNewOne(bool continueOnError = false)
        {
            await FillInDataAsync();

            _logger.Info("Start adding new GtvApi Items to Firestore ...");

            foreach (var itemFromApi in _apiItems)
            {
                try
                {
                    // Add only new one.
                    await AddDtoFirestore(itemFromApi, _firestoreItems);
                }
                catch (Exception ex)
                {
                    // Handle the error here, such as logging the error or displaying a message
                    _logger.Error($"Error occurred while adding item code {_apiItems} to Firestore: {ex.Message}");

                    if (!continueOnError)
                    {
                        throw new FirestoreException($"Error occurred while adding objects to Firestore: {ex.Message}", ex); // Rethrow the exception to stop the execution
                    }
                }
            }

            //for (int i = 0; i < 5; i++)
            //    await AddDtoFirestore(_apiItems[i], _apiItems, _firestoreItems);            

            _logger.Info("GtvApi new Items adding to Firestore completed");
            
            // create new contents
            await CreateContents(_apiItems);
            _logger.Info("GtvApi create new Contents");
        }

        /// <summary>
        /// Sync all items available in the GTV API
        /// 
        /// Update if exists, add if not exists
        /// </summary>
        public async override Task SyncFirestoreAsync(bool continueOnError = false, bool skipApiNullData = true)
        {
            await FillInDataAsync();

            await AddNewOne(continueOnError);

            //for (int i = 0; i < 5; i++)
            //    await AddDtoFirestore(_apiItems[i], _apiItems, _firestoreItems);

            _logger.Info("GtvApi new GtvItem add with Firestore completed");

            //await SyncPrice(continueOnError, skipApiNullData);
            //await SyncStock(continueOnError, skipApiNullData);
        }
       
        /// <summary>
        /// Sync Prices between API and Firestore
        /// </summary>
        /// <param name="continueOnError">If this is true, don't stop if some records contain exceptions. Default is false</param>
        /// <param name="skipApiNullData">If false, override data in Firestore even if null in API. Default is true</param>
        /// <remarks>
        /// If the document does not exist in Firestore, it will be added, if it is different, it will be updated.
        /// If the data in the API is null and skipApiNullData is false, it will be updated to null in Firestore even if the data exists,  
        /// so if there is data in Firestore, it will be deleted. If there is a problem on the API site with the response data, 
        /// you can disallow the data to be overwritten with a null value. You can adopt a strategy that we can overwrite GtvStockService even if null, 
        /// but not e.g. Attributes, if there is any data, it means that they were rather correct, so why delete them during a API has problem with responses data?
        /// </remarks>
        public async Task SyncPrice(bool continueOnError = false, bool skipApiNullData = true)
        {
            await FillInDataAsync();

            _logger.Info("Syncing GtvApi Prices with Firestore ...");

            await syncElementsAsync("GtvPrice", continueOnError: continueOnError, skipApiNullData: skipApiNullData);

            _logger.Info("Syncing GtvApi Prices with Firestore completed");
        }

        /// <summary>
        /// Sync Stocks between API and Firestore
        /// </summary>
        /// <param name="continueOnError">If this is true, don't stop if some records contain exceptions. Default is false</param>
        /// <param name="skipApiNullData">If false, override data in Firestore even if null in API. Default is true</param>
        /// <remarks>
        /// If the document does not exist in Firestore, it will be added, if it is different, it will be updated.
        /// If the data in the API is null and skipApiNullData is false, it will be updated to null in Firestore even if the data exists,  
        /// so if there is data in Firestore, it will be deleted. If there is a problem on the API site with the response data, 
        /// you can disallow the data to be overwritten with a null value. You can adopt a strategy that we can overwrite GtvStockService even if null, 
        /// but not e.g. Attributes, if there is any data, it means that they were rather correct, so why delete them during a API has problem with responses data?
        /// </remarks>
        public async Task SyncStock(bool continueOnError = false, bool skipApiNullData = true)
        {
            await FillInDataAsync();

            _logger.Info("Syncing GtvApi Stocks with Firestore ...");

            await syncElementsAsync("Stocks", continueOnError: continueOnError, skipApiNullData: skipApiNullData);            

            _logger.Info("Syncing GtvApi Stocks with Firestore completed");
        }

        /// <summary>
        /// Sync Attributes between API and Firestore
        /// 
        /// If attribute is type File, it will upload on the Firebase Storage
        /// </summary>
        /// <param name="continueOnError">If this is true, don't stop if some records contain exceptions. Default is false</param>
        /// <param name="skipApiNullData">If false, override data in Firestore even if null in API. Default is true</param>
        /// <remarks>
        /// If the document does not exist in Firestore, it will be added, if it is different, it will be updated.
        /// If the data in the API is null and skipApiNullData is false, it will be updated to null in Firestore even if the data exists,  
        /// so if there is data in Firestore, it will be deleted. If there is a problem on the API site with the response data, 
        /// you can disallow the data to be overwritten with a null value. You can adopt a strategy that we can overwrite GtvStockService even if null, 
        /// but not e.g. Attributes, if there is any data, it means that they were rather correct, so why delete them during a API has problem with responses data?
        /// </remarks>
        public async Task SyncAttribute(bool continueOnError = false, bool skipApiNullData = true)
        {
            await FillInDataAsync();

            _logger.Info("Syncing GtvApi Attributes with Firestore ...");

            await syncElementsAsync("Attributes", continueOnError: continueOnError, skipApiNullData: skipApiNullData);

            foreach (var item in _apiItems)
            {

                foreach (var attribute in item.Attributes)
                {
                    try
                    {
                        await _firestorageFileHandler.StoreAsync(
                            dto: attribute,
                            fileUrlPrefix: _gtvApiSettings.Value.FileUrlPrefix,
                            directoryNameOnFirestore: _firebaseApiSettings.Value.FirestoreDirectoryForFileToUpload
                            );
                    }
                    catch (Exception ex)
                    {
                        // Handle the error here, such as logging the error or displaying a message
                        _logger.Error($"Error occurred while updating {attribute.DocumentUniqueField} with file {attribute.FileHandler}: {ex.Message}");

                        if (!continueOnError)
                        {
                            throw new FirestoreException($"Error occurred while updating {attribute.DocumentUniqueField} with file {attribute.FileHandler}: {ex.Message}", ex); // Rethrow the exception to stop the execution
                        }
                    }
                }
            }
            _logger.Info("Syncing GtvApi Attributes with Firestore completed");
        }

        /// <summary>
        /// Sync Package Types between API and Firestore
        /// </summary>
        /// <param name="continueOnError">If this is true, don't stop if some records contain exceptions. Default is false</param>
        /// <param name="skipApiNullData">If false, override data in Firestore even if null in API. Default is true</param>
        /// <remarks>
        /// If the document does not exist in Firestore, it will be added, if it is different, it will be updated.
        /// If the data in the API is null and skipApiNullData is false, it will be updated to null in Firestore even if the data exists,  
        /// so if there is data in Firestore, it will be deleted. If there is a problem on the API site with the response data, 
        /// you can disallow the data to be overwritten with a null value. You can adopt a strategy that we can overwrite GtvStockService even if null, 
        /// but not e.g. Attributes, if there is any data, it means that they were rather correct, so why delete them during a API has problem with responses data?
        /// </remarks>
        public async Task SyncPackageType(bool continueOnError = false, bool skipApiNullData = true)
        {
            await FillInDataAsync();

            _logger.Info("Syncing GtvApi Package Types with Firestore ...");

            await syncElementsAsync("PackageTypes", continueOnError: continueOnError, skipApiNullData: skipApiNullData);            

            _logger.Info("Syncing GtvApi Package Types with Firestore completed");
        }

        /// <summary>
        /// Sync Alternative Items between API and Firestore
        /// </summary>
        /// <param name="continueOnError">If this is true, don't stop if some records contain exceptions. Default is false</param>
        /// <param name="skipApiNullData">If false, override data in Firestore even if null in API. Default is true</param>
        /// <remarks>
        /// If the document does not exist in Firestore, it will be added, if it is different, it will be updated.
        /// If the data in the API is null and skipApiNullData is false, it will be updated to null in Firestore even if the data exists,  
        /// so if there is data in Firestore, it will be deleted. If there is a problem on the API site with the response data, 
        /// you can disallow the data to be overwritten with a null value. You can adopt a strategy that we can overwrite GtvStockService even if null, 
        /// but not e.g. Attributes, if there is any data, it means that they were rather correct, so why delete them during a API has problem with responses data?
        /// </remarks>
        public async Task SyncAlternativeItem(bool continueOnError = false, bool skipApiNullData = true)
        {
            await FillInDataAsync();

            _logger.Info("Syncing GtvApi Alternative Items with Firestore ...");

            await syncElementsAsync("AlternateItems", continueOnError: continueOnError, skipApiNullData: skipApiNullData);

            _logger.Info("Syncing GtvApi Alternative Items with Firestore completed");
        }

        /// <summary>
        /// Sync GtvItem between API and Firestore
        /// </summary>
        /// <param name="continueOnError">If this is true, don't stop if some records contain exceptions. Default is false</param>
        /// <param name="skipApiNullData">If false, override data in Firestore even if null in API. Default is true</param>
        /// <remarks>
        /// If the document does not exist in Firestore, it will be added, if it is different, it will be updated.
        /// If the data in the API is null and skipApiNullData is false, it will be updated to null in Firestore even if the data exists,  
        /// so if there is data in Firestore, it will be deleted. If there is a problem on the API site with the response data, 
        /// you can disallow the data to be overwritten with a null value. You can adopt a strategy that we can overwrite GtvStockService even if null, 
        /// but not e.g. Attributes, if there is any data, it means that they were rather correct, so why delete them during a API has problem with responses data?
        /// </remarks>
        public async Task SyncItem(bool continueOnError = false, bool skipApiNullData = true)
        {
            await FillInDataAsync();

            _logger.Info("Syncing GtvApi Items with Firestore ...");

            await syncElementsAsync("GtvItem", continueOnError: continueOnError, skipApiNullData: skipApiNullData);

            _logger.Info("Syncing GtvApi Items with Firestore completed");
        }



        /// <summary>
        /// Get all elements from API endpoints
        /// </summary>
        /// <returns>List<FirestoreItemDto></returns>
        private async Task<List<FirestoreItemDto>> getAllFromApiAsync()
        {
            _logger.Info("Collecting data from the GTV API...");

            List<FirestoreItemDto> allFirestoreItemDtos = new List<FirestoreItemDto>();

            List<ItemDto> allApiItemDtos = new List<ItemDto>();

            // loop over LanguageCode enum
            foreach (LanguageCode languageCode in Enum.GetValues(typeof(LanguageCode)))
            {
                // add to list every item by language code
                allApiItemDtos.AddRange(await _itemService.GetAsync(languageCode));
            }

            //var items = await _itemService.GetAsync();
            //allApiItemDtos.AddRange(items);

            IEnumerable<PriceDto> allApiPriceItemDtos = await _priceService.GetAsync();
            IEnumerable<StockDto> allApiStockItemDtos = await _stockService.GetAsync();
            IEnumerable<AttributeDto> allApiAttributeItemDtos = await _attributeService.GetAsync();
            IEnumerable<PackageTypeDto> allApiPackageTypeItemDtos = await _packageTypeService.GetAsync();
            IEnumerable<AlternativeItemDto> allApiAlternativeItemDtos = await _alternativeItem.GetAsync();

            IEnumerable<CategoryTreeDto> allApiCategoryTreeItemDtos = new List<CategoryTreeDto>();
            //try { allApiCategoryTreeItemDtos = await _categoryTreeService.GetAsync(); } catch { }



            // get item with PL language, we will have all available unique items
            var listOfItemUnique = await _itemService.GetAsync();

            for (int i = 0; i < listOfItemUnique.ToList().Count; i++)
            {
                allFirestoreItemDtos.Add(
                    new GtvFirestoreItemDtoBuilder().Build(
                        itemCode: allApiItemDtos[i].ItemCode,
                        allApiItemDtos: allApiItemDtos,
                        allApiPriceDtos: allApiPriceItemDtos.ToList(),
                        allApiStockDtos: allApiStockItemDtos.ToList(),
                        allApiAttributeDtos: allApiAttributeItemDtos.ToList(),
                        allApiCategoryTreeDtos: allApiCategoryTreeItemDtos.ToList(),
                        allApiPackageTypeDtos: allApiPackageTypeItemDtos.ToList(),
                        allApiAlternativeItemDtos: allApiAlternativeItemDtos.ToList()
                        )
                    );
            }

            return allFirestoreItemDtos;
        }

        /// <summary>
        /// Sync data between API and Firestore record or list of records in FirestoreItemDto
        /// </summary>
        /// <param name="nameOfObjectToSync">The name of FirestoreItemDto property to async between GTV API and Firestore</param>
        /// <param name="continueOnError">If this is true, don't stop if some records contain exceptions. Default is false</param>
        /// <param name="skipApiNullData">If false, override data in Firestore even if null in API. Default is true</param>
        /// <remarks>
        /// If the document does not exist in Firestore, it will be added, if it is different, it will be updated.
        /// If the data in the API is null and skipApiNullData is false, it will be updated to null in Firestore even if the data exists,  
        /// so if there is data in Firestore, it will be deleted. If there is a problem on the API site with the response data, 
        /// you can disallow the data to be overwritten with a null value. You can adopt a strategy that we can overwrite GtvStockService even if null, 
        /// but not e.g. Attributes, if there is any data, it means that they were rather correct, so why delete them during a API has problem with responses data?
        /// </remarks>
        private async Task syncElementsAsync(string nameOfObjectToSync, bool continueOnError = false, bool skipApiNullData = true)
        {
            foreach (var itemFromApi in _apiItems)
            {
                // search item in list of objects from Firestore by API object item code
                var itemFromFirestore = _firestoreItems.Where(x => x.ItemCode == itemFromApi.ItemCode).FirstOrDefault();

                if (
                    itemFromFirestore != null &&  // If item object exists in Firestore
                    itemFromApi.Compare(itemFromFirestore) &&  // Are the same objects
                    !itemFromFirestore.IsEqualTo(itemFromApi)  // Are different each other
                    )
                {
                    // Get the type of the FirestoreItemDto class
                    Type managerType = typeof(FirestoreItemDto);

                    // Get all properties of the FirestoreItemDto class
                    PropertyInfo[] properties = managerType.GetProperties();

                    if (!properties.Any(property => property.Name == nameOfObjectToSync))
                        throw new Exceptions.ArgumentException($"syncElementsAsync - {nameOfObjectToSync} doesn't exist in FirestoreItemDto properties");

                    // Iterate over each property
                    foreach (PropertyInfo property in properties)
                    {
                        try
                        {
                            // If property is the same we want to update
                            if (property.Name == nameOfObjectToSync)
                            {
                                // Get the value of the property from the 'itemFromApi' instance.
                                // Can be single record or list or records.
                                object valueApi = property.GetValue(itemFromApi);

                                // If you want to skip api null data
                                if (skipApiNullData && valueApi != null)
                                    continue;

                                // Get the value of the property from the 'itemFromFirestore' instance.
                                // Can be single record or list or records
                                object valueFirestore = property.GetValue(itemFromFirestore);

                                // If object is List of records
                                if (valueFirestore is IEnumerable<object> firestoreEnumerable && valueApi is IEnumerable<object> apiEnumerable)
                                {
                                    // Compare List with nameOfObjectToSync                                
                                    if (!firestoreEnumerable.SequenceEqual(apiEnumerable))
                                    {
                                        // Update if lists of same objects are different.
                                        // Update itemFromFirestore locally with valueApi (nameOfObjectToSync) data from Api object.
                                        property.SetValue(itemFromFirestore, valueApi);
                                    }
                                }
                                // If object is single record compare it between record from api and firestore 'itemFromFirestore' objects
                                else if (valueFirestore == null || valueFirestore != valueApi)
                                {
                                    // Update itemFromFirestore locally with valueApi (nameOfObjectToSync) data from Api object.
                                    property.SetValue(itemFromFirestore, valueApi);
                                }

                                // Update Firestore with updated locally object (itemFromFirestore) by API data
                                await FirestoreService.UpdateDtoAsync(itemFromFirestore);

                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle the error here, such as logging the error or displaying a message
                            _logger.Error($"Error occurred while syncing {nameOfObjectToSync} for item {itemFromApi.ItemCode}: {ex.Message}");

                            if (!continueOnError)
                            {
                                throw new FirestoreException($"Error occurred while syncing {nameOfObjectToSync} for item {itemFromApi.ItemCode}: {ex.Message}", ex); // Rethrow the exception to stop the execution
                            }
                        }
                    }
                }
            }
        }

    }
}
