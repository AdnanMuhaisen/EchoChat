using Google.Cloud.Firestore.V1;

namespace EchoChat.Core.Application.Abstractions.Firestore;

public interface IFirestoreClientFactory
{
    FirestoreClient GetClient();
}