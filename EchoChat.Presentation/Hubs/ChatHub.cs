using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EchoChat.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public string? ReceiverId { get; set; }

    public override Task OnConnectedAsync() => base.OnConnectedAsync();

    public override Task OnDisconnectedAsync(Exception? exception) => base.OnDisconnectedAsync(exception);
}