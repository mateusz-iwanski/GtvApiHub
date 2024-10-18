using Google.Cloud.Firestore;

namespace GtvApiHub.Firebase
{
    public interface IFirestoreConnector
    {
        void Connect();
        FirestoreDb GetFirestoreDb();
    }
}