using Google.Cloud.Firestore;

namespace GtvApiHub.Firestore
{
    public interface IFirestoreConnector
    {
        void Connect();
        FirestoreDb GetFirestoreDb();
    }
}