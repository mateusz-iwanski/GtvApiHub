using FirebaseManager.Exceptions;
using FirebaseManager.Firestore;
using GtvApiHub.WebApi.DTOs;
using GtvApiHub.WebApi.Services;
using NLog;
using System.Linq;

namespace GtvApiHub.WebApi.FirebaseManagement
{
    /// <summary>
    /// GtvFirestoreSyncItemManager is responsible for syncing items between GTV API and Firestore
    /// </summary>
    public class GtvFirestoreSyncItemManager : FirestoreSyncManager, IFirebaseManger
    {
        private List<FirestoreItemDto> _apiItems { get; set; }
        private List<FirestoreItemDto> _firebaseItems { get; set; }

        private readonly IItem _itemService;
        private readonly IPrice _priceService;
        private readonly IStock _stockService;
        private readonly IAttribute _attributeService;
        private readonly ICategoryTree _categoryTreeService;
        private readonly IPackageType _packageTypeService;

        /// <summary>
        /// Use if you want to sync only item DTOs without rest data (attributes, price, stocks....)
        /// </summary>
        /// <param name="item"></param>
        /// <param name="firestoreService"></param>
        /// <param name="logger"></param>
        public GtvFirestoreSyncItemManager(
            IItem itemService,
            IPrice priceService,
            IStock stockService,
            IAttribute attributeService,
            ICategoryTree categoryTreeService,
            IPackageType packageTypeService,
            IFirestoreService firestoreService,
            ILogger logger)
            : base(firestoreService, logger)
        {
            _itemService = itemService;
            _priceService = priceService;
            _stockService = stockService;
            _attributeService = attributeService;
            _categoryTreeService = categoryTreeService;
            _packageTypeService = packageTypeService;
        }

        /// <summary>
        /// Fill in data from API and Firestore
        /// </summary>
        private async Task FillInData()
        {
            if (_apiItems == null || _firebaseItems == null)
            {
                _apiItems = await getAllFromApiAsync();
                _firebaseItems = await GetAllFromFirestoreAsync(_apiItems);
            }
        }

        /// <summary>
        /// Sync all items available in the GTV API
        /// 
        /// Update if exists, add if not exists
        /// </summary>
        public async override Task SyncFirestoreAsync(bool continueOnError = false)
        {
            await FillInData();

            _logger.Info("Start adding new GtvApi Item with Firestore");

            foreach (var itemFromApi in _apiItems)
                try
                {
                    await AddDtoFirestore(itemFromApi, _apiItems, _firebaseItems);
                }
                catch (Exception ex)
                {
                    // Handle the error here, such as logging the error or displaying a message
                    _logger.Error($"Error occurred while adding objects to Firestore: {ex.Message}");

                    if (!continueOnError)
                    {
                        throw new FirestoreException($"Error occurred while adding objects to Firestore: {ex.Message}", ex); // Rethrow the exception to stop the execution
                    }
                }

            for (int i = 0; i < 5; i++)
                await AddDtoFirestore(_apiItems[i], _apiItems, _firebaseItems);

            _logger.Info("GtvApi new Item add with Firestore completed");

            //await SyncPrice();
            //await SyncStock();
        }

        /// <summary>
        /// Sync Prices between API and Firestore
        /// </summary>
        /// <remarks>
        /// Add if not exists, add if list is empty, update if exists.
        /// If the data in API is null it will not be updated in Firestore, 
        /// so if there is data in Firestore it will not be deleted.
        /// </remarks>
        public async Task SyncPrice(bool continueOnError = false)
        {
            await FillInData();

            _logger.Info("Syncing GtvApi Prices with Firestore ...");

            foreach (var itemFromApi in _apiItems)
            {
                try
                {
                    // search item in list of objects from Firestore by API object item code
                    var itemFromFirebase = _firebaseItems.Where(x => x.ItemCode == itemFromApi.ItemCode).FirstOrDefault();

                    if (
                        itemFromFirebase != null &&  // if item object exists in firebase
                        itemFromApi.Price != null &&  // item in Api object is not null
                        itemFromApi.Compare(itemFromFirebase) &&  // are the same objects
                        !itemFromFirebase.IsEqualTo(itemFromApi)  // are different each other
                        )
                    {
                        // if Firebase Prices are null or Prices in API objects are different
                        if (itemFromFirebase.Price == null || itemFromFirebase.Price != itemFromApi.Price)
                        {
                            // update only Prices
                            await FirestoreService.UpdateDtoAsync(itemFromFirebase with { Price = itemFromApi.Price });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the error here, such as logging the error or displaying a message
                    _logger.Error($"Error occurred while syncing Attributes for item {itemFromApi.ItemCode}: {ex.Message}");

                    if (!continueOnError)
                    {
                        throw new FirestoreException($"Error occurred while syncing Attributes for item {itemFromApi.ItemCode}: {ex.Message}", ex); // Rethrow the exception to stop the execution
                    }
                }
            }

            _logger.Info("Syncing GtvApi Prices with Firestore completed");
        }

        /// <summary>
        /// Sync Stocks between API and Firestore
        /// </summary>
        /// <remarks>
        /// Add if not exists, add if list is empty, update if exists.
        /// If the data in API is null it will not be updated in Firestore, 
        /// so if there is data in Firestore it will not be deleted.
        /// </remarks>
        public async Task SyncStock(bool continueOnError = false)
        {
            await FillInData();

            _logger.Info("Syncing GtvApi Stocks with Firestore ...");

            foreach (var itemFromApi in _apiItems)
            {
                try
                {

                    // search item in list of objects from Firestore by API object item code
                    var itemFromFirebase = _firebaseItems.Where(x => x.ItemCode == itemFromApi.ItemCode).FirstOrDefault();

                    if (
                        itemFromFirebase != null &&  // if item object exists in firebase
                        itemFromApi.Stocks != null &&  // item in Api object is not null
                        itemFromApi.Compare(itemFromFirebase) &&  // are the same objects
                        !itemFromFirebase.IsEqualTo(itemFromApi)  // are different each other
                        )
                    {
                        // if Firebase Stocks are null or Stocks in API objects are different
                        if (itemFromFirebase.Stocks == null || !itemFromFirebase.Stocks.SequenceEqual(itemFromApi.Stocks))
                        {
                            // update only Stocks
                            await FirestoreService.UpdateDtoAsync(itemFromFirebase with { Stocks = itemFromApi.Stocks });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the error here, such as logging the error or displaying a message
                    _logger.Error($"Error occurred while syncing Attributes for item {itemFromApi.ItemCode}: {ex.Message}");

                    if (!continueOnError)
                    {
                        throw new FirestoreException($"Error occurred while syncing Attributes for item {itemFromApi.ItemCode}: {ex.Message}", ex); // Rethrow the exception to stop the execution
                    }
                }
            }

            _logger.Info("Syncing GtvApi Stocks with Firestore completed");
        }

        /// <summary>
        /// Sync Attributes between API and Firestore
        /// </summary>
        /// <remarks>
        /// Add if not exists, add if list is empty, update if exists.
        /// If the data in API is null it will not be updated in Firestore, 
        /// so if there is data in Firestore it will not be deleted.
        /// </remarks>
        public async Task SyncAttribute(bool continueOnError = false)
        {
            await FillInData();

            _logger.Info("Syncing GtvApi Attributes with Firestore ...");

            foreach (var itemFromApi in _apiItems)
            {
                try
                {
                    // search item in list of objects from Firestore by API object item code
                    var itemFromFirebase = _firebaseItems.Where(x => x.ItemCode == itemFromApi.ItemCode).FirstOrDefault();

                    if (
                        itemFromFirebase != null &&  // if item object exists in firebase
                        itemFromApi.Attributes != null &&  // item in Api object is not null
                        itemFromApi.Compare(itemFromFirebase) &&  // are the same objects
                        !itemFromFirebase.IsEqualTo(itemFromApi)  // are different each other
                        )
                    {
                        // if Firebase Attributes are null or Attributes in API objects are different
                        if (itemFromFirebase.Attributes == null || !itemFromFirebase.Attributes.SequenceEqual(itemFromApi.Attributes))
                        {
                            // update only Attributes
                            await FirestoreService.UpdateDtoAsync(itemFromFirebase with { Attributes = itemFromApi.Attributes });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the error here, such as logging the error or displaying a message
                    _logger.Error($"Error occurred while syncing Attributes for item {itemFromApi.ItemCode}: {ex.Message}");

                    if (!continueOnError)
                    {
                        throw new FirestoreException($"Error occurred while syncing Attributes for item {itemFromApi.ItemCode}: {ex.Message}", ex); // Rethrow the exception to stop the execution // Rethrow the exception to stop the execution
                    }
                }
            }

            _logger.Info("Syncing GtvApi Attributes with Firestore completed");
        }

        /// <summary>
        /// Sync Package Types between API and Firestore
        /// </summary>
        /// <remarks>
        /// Add if not exists, add if list is empty, update if exists.
        /// If the data in API is null it will not be updated in Firestore, 
        /// so if there is data in Firestore it will not be deleted.
        /// </remarks>
        public async Task SyncPackageType(bool continueOnError = false)
        {
            await FillInData();

            _logger.Info("Syncing GtvApi Package Types with Firestore ...");

            foreach (var itemFromApi in _apiItems)
            {
                try
                {
                    // search item in list of objects from Firestore by API object item code
                    var itemFromFirebase = _firebaseItems.Where(x => x.ItemCode == itemFromApi.ItemCode).FirstOrDefault();

                    if (
                        itemFromFirebase != null &&  // if item object exists in firebase
                        itemFromApi.PackageTypes != null &&  // item in Api object is not null
                        itemFromApi.Compare(itemFromFirebase) &&  // are the same objects
                        !itemFromFirebase.IsEqualTo(itemFromApi)  // are different each other
                        )
                    {
                        // if Firebase Package are null or Package in API objects are different
                        if (itemFromFirebase.PackageTypes == null || !itemFromFirebase.PackageTypes.SequenceEqual(itemFromApi.PackageTypes))
                        {
                            // update only Package
                            await FirestoreService.UpdateDtoAsync(itemFromFirebase with { PackageTypes = itemFromApi.PackageTypes });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the error here, such as logging the error or displaying a message
                    _logger.Error($"Error occurred while syncing Package Types for item {itemFromApi.ItemCode}: {ex.Message}");

                    if (!continueOnError)
                    {
                        throw new FirestoreException($"Error occurred while syncing Package Types for item {itemFromApi.ItemCode}: {ex.Message}", ex); // Rethrow the exception to stop the execution
                    }
                }
            }

            _logger.Info("Syncing GtvApi Package Types with Firestore completed");
        }

        /// <summary>
        /// Sync Item between API and Firestore
        /// </summary>
        /// <remarks>
        /// Add if not exists, add if list is empty, update if exists.
        /// If the data in API is null it will not be updated in Firestore, 
        /// so if there is data in Firestore it will not be deleted.
        /// </remarks>
        public async Task SyncItem(bool continueOnError = false)
        {
            await FillInData();

            _logger.Info("Syncing GtvApi Items with Firestore ...");

            foreach (var itemFromApi in _apiItems)
            {
                try
                {
                    // search item in list of objects from Firestore by API object item code
                    var itemFromFirebase = _firebaseItems.Where(x => x.ItemCode == itemFromApi.ItemCode).FirstOrDefault();

                    if (
                        itemFromFirebase != null &&  // if item object exists in firebase
                        itemFromApi.Item != null &&  // item in Api object is not null
                        itemFromApi.Compare(itemFromFirebase) &&  // are the same objects
                        !itemFromFirebase.IsEqualTo(itemFromApi)  // are different each other
                        )
                    {
                        // if Firebase items are null or items in API objects are different
                        if (itemFromFirebase.Item == null || !itemFromFirebase.Item.SequenceEqual(itemFromApi.Item))
                        {
                            // update only Items
                            await FirestoreService.UpdateDtoAsync(itemFromFirebase with { Item = itemFromApi.Item });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle the error here, such as logging the error or displaying a message
                    _logger.Error($"Error occurred while syncing Package Types for item {itemFromApi.ItemCode}: {ex.Message}");

                    if (!continueOnError)
                    {
                        throw new FirestoreException($"Error occurred while syncing Package Types for item {itemFromApi.ItemCode}: {ex.Message}", ex); // Rethrow the exception to stop the execution
                    }
                }
            }

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
            //foreach (LanguageCode languageCode in Enum.GetValues(typeof(LanguageCode)))
            //{
            //    // add to list every item by language code
            //    var items = await _itemService.GetAsync(languageCode);
            //    allApiItemDtos.AddRange(items);
            //}

            var items = await _itemService.GetAsync();
            allApiItemDtos.AddRange(items);

            IEnumerable<PriceDto> allApiPriceItemDtos = await _priceService.GetAsync();
            IEnumerable<StockDto> allApiStockItemDtos = await _stockService.GetAsync();
            IEnumerable<AttributeDto> allApiAttributeItemDtos = await _attributeService.GetAsync();
            IEnumerable<PackageTypeDto> allApiPackageTypeItemDtos = await _packageTypeService.GetAsync();

            IEnumerable<CategoryTreeDto> allApiCategoryTreeItemDtos = new List<CategoryTreeDto>();
            //try { allApiCategoryTreeItemDtos = await _categoryTreeService.GetAsync(); } catch { }



            // get item with PL language, we will have all available unique items
            var listOfItemUnique = await _itemService.GetAsync();

            for (int i = 0; i < listOfItemUnique.ToList().Count; i++)
            {
                allFirestoreItemDtos.Add(
                    new GtvFirebaseItemDtoBuilder().Build(
                        itemCode: allApiItemDtos[i].ItemCode,
                        allApiItemDtos: allApiItemDtos,
                        allApiPriceDtos: allApiPriceItemDtos.ToList(),
                        allApiStockDtos: allApiStockItemDtos.ToList(),
                        allApiAttributeDtos: allApiAttributeItemDtos.ToList(),
                        allApiCategoryTreeDtos: allApiCategoryTreeItemDtos.ToList(),
                        allApiPackageTypeDtos: allApiPackageTypeItemDtos.ToList()
                        )
                    );
            }

            return allFirestoreItemDtos;

        }


    }
}
