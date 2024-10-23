using FirebaseManager.Firestore;
using Google.Cloud.Firestore;
using GtvApiHub.WebApi.DTOs;
using GtvApiHub.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace GtvApiHub.WebApi.FirebaseManagement
{
    public class GtvFirebaseSyncItemManager : FirebasSyncManager, IFirebaseManger
    {
        private readonly IItem _itemService;

        public GtvFirebaseSyncItemManager(IItem item, IFirestoreService firestoreService, ILogger logger) 
            : base(firestoreService, logger)
        {
            _itemService = item;
        }

        public async override Task SyncAsync()
        {
            var apiItems = await getAllFromApiAsync();
            var firebaseItems = await getAllFromFirebaseAsync(apiItems);

            foreach (var itemDtoFromApi in apiItems)
                await updateOrAddDtoToFirebase(itemDtoFromApi, apiItems, firebaseItems);
        }

        private async Task<List<ItemDto>> getAllFromFirebaseAsync(List<ItemDto> allItemsFromApi)
        {
            List<ItemDto> itemDto = new List<ItemDto>();

            var collections = allItemsFromApi.Select(x => x.CollectionName).Distinct();

            foreach (var collection in collections)
            {
                var itemsInCollection = await FirebaseGetAllDocumentsInCollectionAsync<ItemDto>(collection);
                itemDto.AddRange(itemsInCollection);
            }
            
            return itemDto;
        }

        private async Task<List<ItemDto>> getAllFromApiAsync()
        {
            var items = await _itemService.GetAsync();
            return items.ToList();
        }

        
    }
}
