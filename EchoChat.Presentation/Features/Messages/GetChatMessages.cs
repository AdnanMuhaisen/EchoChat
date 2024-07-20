using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Core.Domain.Common;
using EchoChat.Core.Domain.Common.Requirements;
using EchoChat.Core.Domain.MessageAggregate;
using EchoChat.Dtos;
using Mapster;
using MediatR;

namespace EchoChat.Features.Messages;

public static class GetChatMessages
{
    public class Query(string? chatId) : IRequest<List<MessageDto>>
    {
        public string? ChatId { get; } = chatId;
    }

    public sealed class Handler(ICollectionReferenceFactory collectionReferenceFactory) : IRequestHandler<Query, List<MessageDto>>
    {
        public async Task<List<MessageDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<MessageDto> chatMessages = [];
            var messagesCollection = collectionReferenceFactory.GetCollection(FirbaseRequirements.MessagesCollectionPath);
            var documents = await messagesCollection
                .WhereEqualTo("ChatId", request.ChatId)
                .WhereEqualTo("IsDeleted", false)
                .GetSnapshotAsync(cancellationToken);

            foreach (var message in documents)
            {
                var messageDto = message.ConvertTo<Message>().Adapt<MessageDto>();
                messageDto.SentAt = messageDto.SentAt.ToJordanDateTime();
                messageDto.SeenAt = messageDto.SeenAt is not null
                    ? ((DateTime)messageDto.SeenAt).ToJordanDateTime()
                    : messageDto.SeenAt;
                chatMessages.Add(messageDto);
            }

            return chatMessages;
        }
    }
}