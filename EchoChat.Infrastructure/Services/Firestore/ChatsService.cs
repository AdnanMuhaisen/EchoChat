using EchoChat.Application.Abstractions.Firestore;
using EchoChat.Domain.ChatAggregates;
using EchoChat.Domain.Common.Requirements;
using Google.Cloud.Firestore;

namespace EchoChat.Infrastructure.Services.Firestore;

public class ChatsService : IChatsService
{
    private CollectionReference _collection;

    public ChatsService(IFirestoreClientFactory firestoreClientFactory)
    {
        _collection = FirestoreDb.Create(FirestoreRequirements.ProjectId, firestoreClientFactory.GetClient())
            .Collection(FirestoreRequirements.ChatsCollection);
    }

    public Task<bool> AddAsync(Chat chat) => throw new NotImplementedException();
    public Task<bool> DeleteAsync(string documentId) => throw new NotImplementedException();
    public Task<List<Chat>> GetAsync(string userId) => throw new NotImplementedException();
    public Task<bool> UpdateAsync() => throw new NotImplementedException();
}