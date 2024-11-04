using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.DTOs
{
    /// <summary>
    /// Used in ContentsDto
    /// </summary>
    [FirestoreData]
    public record ItemCodePair
    {
        [FirestoreProperty]
        public int Id { get; init; }

        [FirestoreProperty]
        public string ItemCode { get; init; }
    }
}
