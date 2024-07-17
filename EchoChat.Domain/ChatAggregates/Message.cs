using EchoChat.Domain.Abstractions;
using Google.Cloud.Firestore;

namespace EchoChat.Domain.ChatAggregates;

[FirestoreData]
public class Message : IDeletableEntity
{
    [FirestoreProperty]
    public string? SenderId { get; set; }

    [FirestoreProperty]
    public string? Text { get; set; }

    [FirestoreProperty]
    public int Index { get; set; }

    [FirestoreProperty]
    public DateTime SentAt { get; set; }

    [FirestoreProperty]
    public DateTime? SeenAt { get; set; }

    [FirestoreProperty]
    public bool IsDeleted { get; set; }

    [FirestoreProperty]
    public DateTime? DeletedAt { get; set; }

    [FirestoreProperty]
    public string? FileId { get; set; }
}