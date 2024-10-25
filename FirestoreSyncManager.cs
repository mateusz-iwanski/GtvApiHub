using FirebaseManager.Firestore;
using Google.Cloud.Firestore;
using GtvApiHub.WebApi.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using GtvApiHub.WebApi;

namespace GtvApiHub
{
    public abstract class FirestoreSyncManager : IFirestoreManager
    {
        protected readonly IFirestoreService FirestoreService;
        private readonly FirestoreDb _firestoreDb;
        protected readonly ILogger _logger;

        public FirestoreSyncManager(IFirestoreService firestoreService, ILogger logger)
        {
            FirestoreService = firestoreService;
            _firestoreDb = FirestoreService.GetFirestoreDb();
            _logger = logger;
        }

        /// <summary>
        /// Sync all items available in the GTV API
        /// </summary>
        public abstract Task SyncFirestoreAsync(bool continueOnError, bool skipApiNullData = true);

        public virtual async Task<List<T>> GetAllFirestoreDocumentsInCollectionAsync<T>(string collectionName) 
            where T : IFirestoreDto
        {
            CollectionReference collection = _firestoreDb.Collection(collectionName);
            QuerySnapshot snapshot = await collection.GetSnapshotAsync();
            List<T> documents = snapshot.Documents.Select(doc => doc.ConvertTo<T>()).ToList();
            return documents;
        }

        /// <summary>
        /// If DTO with unique field in collection exists, return true.
        /// If DTO doesn't have unique field, return null.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<bool?> CheckIfFirestoreDocumentExistsAsync(IFirestoreDto dto)
        {
            if (dto.DocumentUniqueField == null)
                return null;

            DocumentReference document = _firestoreDb.Collection(dto.CollectionName).Document(dto.DocumentUniqueField);
            return await FirestoreService.IsDtoExistsAsync(document);
        }

        /// <summary>
        /// Download all documents (DTO) from Firebase from all collections with this document type.
        /// </summary>
        /// <typeparam name="T">IFirestoreDto</typeparam>
        /// <param name="allDtoFromApi">List<IFirestoreDto></param>
        /// <returns>List<IFirestoreDto></returns>
        public async Task<List<T>> GetAllFromFirestoreAsync<T>(List<T> allDtoFromApi) where T : IFirestoreDto
        {
            _logger.Info("Collecting data from the Firestore...");

            List<T> itemDto = new List<T>();

            // get names of all collections
            var collections = allDtoFromApi.Select(x => x.CollectionName).Distinct();

            // get all documents from all collections
            foreach (var collection in collections)
            {
                // get documents by collection from firestore
                var itemsInCollection = await GetAllFirestoreDocumentsInCollectionAsync<T>(collection);
                itemDto.AddRange(itemsInCollection);
            }

            return itemDto;
        }

        protected async Task UpdateOrAddDtoFirestore<T>(
            IFirestoreDtoCompareStrategy itemDtoFromApi,
            List<T> firebaseItems
            ) where T : IFirestoreDto
        {
            foreach (var item in firebaseItems)
            {
                // if item exists in Firestore
                if (itemDtoFromApi.Compare(item))
                {
                    // if is updated in api
                    if (itemDtoFromApi.IsUpdated(item))
                    {
                        await FirestoreService.UpdateDtoAsync((IFirestoreDto)itemDtoFromApi);
                        break;
                    }
                }
            }

            // Add to firestore if item does not exist in Firestore
            await FirestoreService.InsertDtoAsync((IFirestoreDto)itemDtoFromApi);
        }

        protected async Task AddDtoFirestore<T>(
            IFirestoreDtoCompareStrategy itemDtoFromApi,
            List<T> firebaseItems
            ) where T : IFirestoreDto
        {
            foreach (var item in firebaseItems)
            {
                // if item exists in Firestore
                if (itemDtoFromApi.Compare(item))
                    break;
            }

            // not exist in Firestore so add
            await FirestoreService.InsertDtoAsync((IFirestoreDto)itemDtoFromApi);
        }

    }
}
