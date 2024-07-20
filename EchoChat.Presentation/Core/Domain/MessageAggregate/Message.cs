using EchoChat.Core.Domain.Abstractions;
using Google.Cloud.Firestore;

namespace EchoChat.Core.Domain.MessageAggregate;

[FirestoreData]
public class Message : IDeletableEntity
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty]
    public string? ChatId { get; set; }

    [FirestoreProperty]
    public string? SenderId { get; set; } 
    
    [FirestoreProperty]
    public string? ReceiverId { get; set; }

    [FirestoreProperty]
    public string? Text { get; set; }

    [FirestoreProperty]
    public DateTime SentAt { get; set; }

    [FirestoreProperty]
    public DateTime? SeenAt { get; set; }

    [FirestoreProperty]
    public bool IsDeleted { get; set; }

    [FirestoreProperty]
    public DateTime? DeletedAt { get; set; }

    [FirestoreProperty]
    public MessageFile? MessageFile { get; set; }
}