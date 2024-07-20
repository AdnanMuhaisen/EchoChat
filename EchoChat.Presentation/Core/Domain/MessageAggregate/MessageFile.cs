using Google.Cloud.Firestore;

namespace EchoChat.Core.Domain.MessageAggregate;

[FirestoreData]
public class MessageFile
{
    [FirestoreProperty]
    public string? Url{ get; set; }

    [FirestoreProperty]
    public string? ContentType { get; set; }
}