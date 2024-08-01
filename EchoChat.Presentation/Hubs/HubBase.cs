using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EchoChat.Hubs;

public class HubBase : Hub
{
    protected static Dictionary<string, List<string>> UserConnections = [];

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
            UserConnections.Add(userId!, new List<string>() { Context.ConnectionId });
        }

        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // log the exception message
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

        throw new InvalidOperationException("There`s no connection for the speceified user");
    }
}