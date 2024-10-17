using Google.Cloud.Firestore;
using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.Firestore
{
    public interface IFirestoreService
    {
        Task<bool> InsertDto(IFirestoreDto dto);
        Task<bool> UpdateDto(IFirestoreDto dto);
        Task<bool> DeleteDto(IFirestoreDto dto);
    }
}