using Google.Cloud.Firestore;

namespace EchoChat.Core.Application.Abstractions.Firestore;

public interface ICollectionReferenceFactory
{
    CollectionReference GetCollection(string path);
}