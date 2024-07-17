using Google.Cloud.Firestore.V1;

namespace EchoChat.Application.Abstractions.Firestore;

public interface IFirestoreClientFactory
{
    FirestoreClient GetClient();
}