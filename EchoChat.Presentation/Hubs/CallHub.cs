using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EchoChat.Hubs;

[Authorize]
public class CallHub : HubBase
{
    public async Task SendIceCandidate(string iceCandidate, string receiverId)
    {
        if (!UserConnections.TryGetValue(receiverId, out _))
        {
            await Clients.Caller.SendAsync("callingOfflineUser");
            return;
        }

        await Clients.User(receiverId).SendAsync("receiveIceCandidate", iceCandidate);
    }

    public async Task SendOffer(string offer, string senderId, string receiverId, string receiverName)
    {
        if (!UserConnections.TryGetValue(receiverId, out _))
        {
            await Clients.Caller.SendAsync("callingOfflineUser");
            return;
        }

        await Clients.User(receiverId).SendAsync("receiveOffer", offer, senderId, receiverName);
    }

    public async Task SendAnswer(string answer, string senderId)
    {
        if (!UserConnections.TryGetValue(senderId, out _))
        {
            await Clients.Caller.SendAsync("callingOfflineUser");
            return;
        }

        await Clients.User(senderId).SendAsync("receiveAnswer", answer);
    }

    public async Task EndCall(string receiverId)
    {
        await Clients.User(receiverId).SendAsync("callEnded");
    }
}