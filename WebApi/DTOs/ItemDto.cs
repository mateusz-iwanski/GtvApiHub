using FirebaseManager.Firestore;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.DTOs
{
    /// <summary>
    /// Item DTO represents products from Api.
    /// </summary>
    [FirestoreData]
    public record ItemDto : IBaseDto, IResponseDto, IFirestoreDto
    {
        [FirestoreProperty]
        public string ItemCode { get; init; }

        [FirestoreProperty]
        public string ItemName { get; init; }
        
        [FirestoreProperty]
        public string LanguageCode { get; init; }

        public string CollectionName { get => "items"; }

        public string DocumentUniqueField { get => ItemCode; }
    }
}
