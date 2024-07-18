using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Core.Domain.ChatAggregates;
using EchoChat.Core.Domain.Common.Requirements;
using EchoChat.Dtos;
using Mapster;
using MediatR;

namespace EchoChat.Features.Chats;

public static class GetAllChats
{
    public class Query(string userId) : IRequest<List<ChatDto>>
    {
        public string? UserId { get; set; } = userId;
    }

    public sealed class Handler(ICollectionReferenceFactory collectionReferenceFactory) : IRequestHandler<Query, List<ChatDto>>
    {
        public async Task<List<ChatDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<ChatDto> chats = [];
            var chatsCollectionReference = collectionReferenceFactory.GetCollection(FirestoreRequirements.ChatsCollectionPath);
            var userChats = await chatsCollectionReference
                .WhereEqualTo("UserId", request.UserId)
                .WhereEqualTo("IsDeleted", false)
                .GetSnapshotAsync();

            foreach (var chat in userChats.Documents)
            {
                chats.Add(chat.ConvertTo<Chat>().Adapt<ChatDto>());
            }

            return chats;
        }
    }
}