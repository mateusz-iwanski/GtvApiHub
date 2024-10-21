using FirebaseManager.Firestore;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.DTOs
{
    [FirestoreData]
    public record PriceDto : IBaseDto, IResponseDto, IFirestoreDto
    {
        [FirestoreProperty]
        public string CardCode { get; init; }

        [FirestoreProperty]
        public string ItemCode { get; init; }

        [FirestoreProperty]
        public decimal BasePrice { get; init; }

        [FirestoreProperty]
        public decimal FinalPrice { get; init; }

        [FirestoreProperty]
        public double Discount { get; init; }

        [FirestoreProperty]
        public string Currency { get; init; }

        public string CollectionName { get => "itemsPrices"; }
        public string DocumentUniqueField { get => ItemCode; }
    } 
}
