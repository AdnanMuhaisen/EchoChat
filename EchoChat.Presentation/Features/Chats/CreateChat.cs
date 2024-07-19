using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Core.Domain.ChatAggregates;
using EchoChat.Core.Domain.Common.Requirements;
using EchoChat.Dtos;
using Mapster;
using MediatR;

namespace EchoChat.Features.Chats;

public static class CreateChat
{
    public class Command(string? firstMemberId, string? firstMemberName, string? secondMemberId, string? secondMemberName) : IRequest<ChatDto>
    {
        public string? FirstMemberId { get; set; } = firstMemberId;

        public string? FirstMemberName { get; set; } = firstMemberName;

        public string? SecondMemberId { get; set; } = secondMemberId;

        public string? SecondMemberName { get; set; } = secondMemberName;
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