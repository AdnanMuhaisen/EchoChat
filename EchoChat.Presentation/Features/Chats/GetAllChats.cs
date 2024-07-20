using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Core.Domain.ChatAggregates;
using EchoChat.Core.Domain.Common.Requirements;
using EchoChat.Dtos;
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
            var chatsCollectionReference = collectionReferenceFactory.GetCollection(FirbaseRequirements.ChatsCollectionPath);
            var userChats = await chatsCollectionReference
                .WhereEqualTo("FirstMemberId", request.UserId)
                .WhereEqualTo("IsDeleted", false)
                .GetSnapshotAsync();

            var userReceivedChats = await chatsCollectionReference
                .WhereEqualTo("SecondMemberId", request.UserId)
                .WhereEqualTo("IsDeleted", false)
                .GetSnapshotAsync();

            foreach (var chatDocument in userChats.Documents.Concat(userReceivedChats.Documents))
            {
                ChatDto chatDto;
                var chat = chatDocument.ConvertTo<Chat>();
                if (request.UserId == chat.FirstMemberId)
                {
                    // the first member is the sender and the second member is the receiver
                    chatDto = new ChatDto
                    {
                        Id = chat.Id,
                        UserId = chat.FirstMemberId,
                        UserName = chat.FirstMemberName,
                        ReceiverId = chat.SecondMemberId,
                        ReceiverName = chat.SecondMemberName
                    };
                }
                else
                {
                    // the first member is the receiver and the second member is the sender
                    chatDto = new ChatDto
                    {
                        Id = chat.Id,
                        UserId = chat.SecondMemberId,
                        ReceiverId = chat.FirstMemberId,
                        UserName = chat.SecondMemberName,
                        ReceiverName = chat.FirstMemberName
                    };
                }

                chats.Add(chatDto);
            }

            return chats;
        }
    }
}