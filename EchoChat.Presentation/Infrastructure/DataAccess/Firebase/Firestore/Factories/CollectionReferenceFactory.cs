using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Core.Domain.Common.Requirements;
using Google.Cloud.Firestore;

namespace EchoChat.Infrastructure.DataAccess.Firebase.Firestore.Factories;

public class CollectionReferenceFactory(IFirestoreClientFactory firestoreClientFactory) : ICollectionReferenceFactory
{
    public CollectionReference GetCollection(string path)
        => FirestoreDb.Create(FirbaseRequirements.ProjectId, firestoreClientFactory.GetClient()).Collection(path);
}