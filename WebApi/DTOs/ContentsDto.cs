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
    /// Store the id documents with the item code inside documents.
    /// 
    /// If you want to find item code or document ID, you do not have to search all documents,
    /// just find it over id or item code from ContentsDto and read just one document.
    /// Firestore has reading limits so the best option is using this DTO to find object over
    /// item code or document ID.
    /// </summary>
    [FirestoreData]
    public class ContentsDto : IBaseDto, IResponseDto, IFirestoreDto
    {
        [FirestoreProperty]
        public List<ItemCodePair> ItemCodePairs { get; init; }

        public string CollectionName => "Gtv_Items_Contents";

        public string DocumentUniqueField => "Contents";
    }
}
