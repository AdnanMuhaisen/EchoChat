using EchoChat.Domain.ChatAggregates;

namespace EchoChat.Application.Abstractions.Firestore;

public interface IChatsService
{
    Task<List<Chat>> GetAsync(string userId);

    Task<bool> AddAsync(Chat chat);

    Task<bool> UpdateAsync();

    Task<bool> DeleteAsync(string documentId);
}