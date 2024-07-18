using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Core.Domain.ChatAggregates;
using EchoChat.Core.Domain.Common.Requirements;
using EchoChat.Dtos;
using Mapster;
using MediatR;

namespace EchoChat.Features.Chats;

public static class CreateChat
{
    public class Command(string? userId, string? receiverId, string? receiverName) : IRequest<ChatDto>
    {
        public string? UserId { get; set; } = userId;

        public string? ReceiverId { get; set; } = receiverId;

        public string? ReceiverName { get; set; } = receiverName;
    }

    public sealed class Handler(ICollectionReferenceFactory collectionReferenceFactory) : IRequestHandler<Command, ChatDto>
    {
        public async Task<ChatDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var chatsCollection = collectionReferenceFactory.GetCollection(FirestoreRequirements.ChatsCollectionPath);
            Chat addedChat = request.Adapt<Chat>();
            var documentReference = await chatsCollection.AddAsync(addedChat, cancellationToken);
            var documentSnapshot = await documentReference.GetSnapshotAsync(cancellationToken);
            addedChat = documentSnapshot.ConvertTo<Chat>();

            return addedChat.Adapt<ChatDto>();
        }
    }
}