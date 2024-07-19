using EchoChat.Core.Application.Abstractions.Firestore;
using EchoChat.Features.Messages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EchoChat.Hubs;

[Authorize]
public class ChatHub(ISender sender, IConfiguration configuration, IFirebseStorageService firebseStorageService) : Hub
{
    public static Dictionary<string, List<string>> UserConnections = [];

    public override Task OnConnectedAsync()
    {
        var userId = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);
        if (UserConnections.TryGetValue(userId!, out List<string>? userConnection))
        {
            userConnection.Add(Context.ConnectionId);
            UserConnections[userId!] = userConnection;
        }
        else
        {
            UserConnections.Add(userId!, [Context.ConnectionId]);
        }

        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);
        if (UserConnections.TryGetValue(userId!, out List<string>? userConnections))
        {
            if (userConnections.Count() == 1)
            {
                UserConnections.Remove(userId!);
                return Task.CompletedTask;
            }
            else
            {
                UserConnections[userId!] = userConnections;
                return Task.CompletedTask;
            }
        }
        // else
        throw new InvalidOperationException("There`s no connection for the speceified user");
    }

    public async Task SendMessageAsync(string chatId, string receiverId, string receiverName, string message, string fileAsBase64String, string fileName, string contentType)
    {
        var userId = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!string.IsNullOrEmpty(fileAsBase64String) && !string.IsNullOrEmpty(fileName))
        {
            var generatedFileName = $"{Guid.NewGuid().ToString().Split('-')[0]}-{fileName}";
            await firebseStorageService.UploadFileAsync(generatedFileName, fileAsBase64String, contentType);


        }

        var addedMessage = await sender
            .Send(new CreateMessage.Command(chatId, userId, receiverId, message, DateTime.UtcNow, DateTime.UtcNow, null));
        if (!string.IsNullOrEmpty(addedMessage.Id))
        {
            await Clients.User(receiverId).SendAsync("receiveMessage", receiverName, addedMessage.Text, addedMessage.SentAt.ToShortTimeString());
            await Clients.Caller.SendAsync("displayTheSentMessage", addedMessage.Text, addedMessage.SentAt.ToShortTimeString());
            await Clients.User(receiverId).SendAsync("hideTypingMessage");
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