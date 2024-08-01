using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EchoChat.Hubs;

[Authorize]
public class CallHub : HubBase
{
    public async Task CallUser(string receiverId, string callType)
    {
        var (senderId, senderName) = (Context.User!.FindFirstValue(ClaimTypes.NameIdentifier), Context.User!.FindFirstValue(ClaimTypes.Name));
        await Clients.User(receiverId).SendAsync("comingCall", senderId, senderName, callType);
    }

    public async Task RejectComingCall(string receiverId)
        => await Clients.User(receiverId).SendAsync("rejectedCall");

    public async Task SendIceCandidate(string iceCandidate, string receiverId)
    {
        if (!UserConnections.TryGetValue(receiverId, out _))
        {
            await Clients.Caller.SendAsync("callingOfflineUser");
            return;
        }

        await Clients.User(receiverId).SendAsync("receiveIceCandidate", iceCandidate);
    }

    public async Task SendOffer(string offer, string senderId, string receiverId, string receiverName, bool isVideoCall = false)
    {
        if (!UserConnections.TryGetValue(receiverId, out _))
        {
            await Clients.Caller.SendAsync("callingOfflineUser");
            return;
        }

        await Clients.User(receiverId).SendAsync("receiveOffer", offer, senderId, receiverName, isVideoCall);
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

    public async Task EndCall(string receiverId, bool isVideoCall)
       => await Clients.User(receiverId).SendAsync("callEnded", isVideoCall);
}