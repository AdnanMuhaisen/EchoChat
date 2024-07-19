using EchoChat.Core.Domain.Abstractions;
using Google.Cloud.Firestore;

namespace EchoChat.Core.Domain.ChatAggregates;

[FirestoreData]
public class Chat : IFirestoreEntity, IDeletableEntity
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty]
    public string? FirstMemberId { get; set; }

    [FirestoreProperty]
    public string? SecondMemberId { get; set; }

    [FirestoreProperty]
    public string? FirstMemberName { get; set; }

    [FirestoreProperty]
    public string? SecondMemberName { get; set; }

    [FirestoreProperty]
    public bool IsDeleted { get; set; }

    [FirestoreProperty]
    public DateTime? DeletedAt { get; set; }
}