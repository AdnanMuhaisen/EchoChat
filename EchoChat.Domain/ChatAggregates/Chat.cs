using EchoChat.Domain.Abstractions;
using Google.Cloud.Firestore;

namespace EchoChat.Domain.ChatAggregates;

[FirestoreData]
public class Chat : IFirestoreEntity, IDeletableEntity
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty]
    public string? UserId { get; set; }

    [FirestoreProperty]
    public string? ReceiverId { get; set; }

    [FirestoreProperty]
    public List<Message> Messages { get; set; } = [];

    [FirestoreProperty]
    public bool IsDeleted { get; set; }

    [FirestoreProperty]
    public DateTime? DeletedAt { get; set; }
}