using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Core.Domain.Common;
using EchoChat.Features.Messages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EchoChat.Hubs;

[Authorize]
public class ChatHub(ISender sender, IFirebseStorageService firebseStorageService) : HubBase
{
    public async Task SendMessageAsync(string chatId, string receiverId, string message, string fileAsBase64String, string fileName, string contentType)
    {
        await Clients.Caller.SendAsync("showSendingMessage", true);
        var userId = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = Context.User!.FindFirstValue(ClaimTypes.Name);
        var createMessageCommand = new CreateMessage.Command(chatId, userId, receiverId, message, DateTime.UtcNow, DateTime.UtcNow, null);
        if (!string.IsNullOrEmpty(fileAsBase64String) && !string.IsNullOrEmpty(fileName))
        {
            var generatedFileName = $"{Guid.NewGuid().ToString().Split('-')[0]}-{fileName}";
            var messageFile = await firebseStorageService.UploadFileAsync(generatedFileName, fileAsBase64String, contentType);
            if (messageFile is not null)
            {
                createMessageCommand.MessageFile = messageFile;
            }
        }

        var addedMessage = await sender.Send(createMessageCommand);
        if (!string.IsNullOrEmpty(addedMessage.Id))
        {
            await Clients.User(receiverId).SendAsync(
                "receiveMessage",
                userName![..userName!.IndexOf('@')],
                //receiverName,
                addedMessage.Text,
                addedMessage.SentAt.ToJordanDateTime().ToShortTimeString(),
                addedMessage.MessageFile?.Url ?? null,
                addedMessage.MessageFile?.ContentType ?? null);
            await Clients.Caller.SendAsync(
                "displayTheSentMessage",
                addedMessage.Text,
                addedMessage.SentAt.ToJordanDateTime().ToShortTimeString(),
                addedMessage.MessageFile?.Url ?? null,
                addedMessage.MessageFile?.ContentType ?? null);
            await Clients.User(receiverId).SendAsync("hideTypingMessage");
            await Clients.Caller.SendAsync("showSendingMessage", false);
        }

        // error
    }

    public async Task<bool> DisplayTypingMessage(string receiverId)
    {
        if (UserConnections.TryGetValue(receiverId, out _))
        {
            await Clients.User(receiverId).SendAsync("displayTypingMessage");
            return true;
        }

        return false;
    }

    public async Task HideTypingMessage(string receiverId)
          => await Clients.User(receiverId).SendAsync("hideTypingMessage");
}