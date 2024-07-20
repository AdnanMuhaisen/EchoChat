using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Core.Domain.Common.Requirements;
using EchoChat.Core.Domain.MessageAggregate;
using EchoChat.Dtos;
using Mapster;
using MediatR;

namespace EchoChat.Features.Messages;

public static class CreateMessage
{
    public class Command(
        string? chatId,
        string? senderId,
        string? receiverId,
        string? text,
        DateTime sentAt,
        DateTime? seenAt,
        MessageFile? messageFile)
        : IRequest<MessageDto>
    {
        public string? ChatId { get; set; } = chatId;

        public string? SenderId { get; set; } = senderId;

        public string? ReceiverId { get; set; } = receiverId;

        public string? Text { get; set; } = text;

        public DateTime SentAt { get; set; } = sentAt;

        public DateTime? SeenAt { get; set; } = seenAt;

        public MessageFile? MessageFile { get; set; } = messageFile;
    }

    public sealed class Handler(ICollectionReferenceFactory collectionReferenceFactory) : IRequestHandler<Command, MessageDto>
    {
        public async Task<MessageDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var messagesCollectionReference = collectionReferenceFactory.GetCollection(FirbaseRequirements.MessagesCollectionPath);
            var addedMessage = request.Adapt<Message>();
            var documentReference = await messagesCollectionReference.AddAsync(addedMessage, cancellationToken);
            var documentSnapshot = await documentReference.GetSnapshotAsync(cancellationToken);

            return documentSnapshot.ConvertTo<Message>().Adapt<MessageDto>();
        }
    }
}