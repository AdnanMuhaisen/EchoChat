using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EchoChat.Hubs;

[Authorize]
public class CallHub : HubBase
{
    public async Task SendIceCandidate(string iceCandidate, string receiverId)
    {
        await Clients.User(receiverId).SendAsync("receiveIceCandidate", iceCandidate);
    }

    public async Task SendOffer(string offer, string senderId, string receiverId)
    {
        await Clients.User(receiverId).SendAsync("receiveOffer", offer, senderId);
    }

    public async Task SendAnswer(string answer, string senderId)
    {
        await Clients.User(senderId).SendAsync("receiveAnswer", answer);
    }
}