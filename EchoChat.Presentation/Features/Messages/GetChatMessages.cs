using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Core.Domain.Common.Requirements;
using EchoChat.Core.Domain.MessageAggregate;
using EchoChat.Dtos;
using Mapster;
using MediatR;

namespace EchoChat.Features.Messages;

public static class GetChatMessages
{
    public class Query(string? chatId, string? userId, string? receiverId) : IRequest<List<MessageDto>>
    {
        public string? ChatId { get; } = chatId;

        public string? UserId { get; set; } = userId;

        public string? ReceiverId { get; set; } = receiverId;
    }

    public sealed class Handler(ICollectionReferenceFactory collectionReferenceFactory) : IRequestHandler<Query, List<MessageDto>>
    {
        public async Task<List<MessageDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<MessageDto> chatMessages = [];
            var messagesCollection = collectionReferenceFactory.GetCollection(FirestoreRequirements.MessagesCollectionPath);
            var documents = await messagesCollection
                .WhereEqualTo("ChatId", request.ChatId)
                .WhereEqualTo("UserId", request.UserId)
                .WhereEqualTo("ReceiverId", request.ReceiverId)
                .GetSnapshotAsync(cancellationToken);

            foreach (var message in documents)
            {
                chatMessages.Add(message.ConvertTo<Message>().Adapt<MessageDto>());
            }

            return chatMessages;
        }
    }
}