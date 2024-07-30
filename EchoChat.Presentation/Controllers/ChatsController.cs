using EchoChat.Dtos;
using EchoChat.Features.Chats;
using EchoChat.Features.Messages;
using EchoChat.Features.Users;
using EchoChat.Models.ViewModels.Chats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EchoChat.Controllers;

[Authorize]
[Route("[controller]")]
public class ChatsController(ISender sender) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userChats = await sender.Send(new GetAllChats.Query(userId!), cancellationToken);
        var usersWithChats = userChats.Select(x => int.Parse(x.ReceiverId!));
        var usersWithoutChats = await sender.Send(new GetUsersWithoutChat.Query(int.Parse(userId!), usersWithChats), cancellationToken);

        return View(new ChatsViewModel
        {
            UserChats = userChats,
            UsersWithoutChats = usersWithoutChats
        });
    }

    [HttpGet("new-chat/{receiverId}/{receiverName}")]
    public async Task<IActionResult> Post([FromRoute] string receiverId, [FromRoute] string receiverName, CancellationToken cancellationToken)
    {
        await sender.Send(new CreateChat.Command(
            User!.FindFirstValue(ClaimTypes.NameIdentifier), User.FindFirstValue(ClaimTypes.Name), receiverId, receiverName),
            cancellationToken);

        return RedirectToAction("Index");
    }

    [HttpGet("{id}/{receiverId}/{receiverName}")]
    public async Task<IActionResult> GetChatMessages([FromRoute(Name = "id")] string chatId, [FromRoute] string receiverId, [FromRoute] string receiverName, CancellationToken cancellationToken)
    {
        List<MessageDto> chatMessages = (await sender
            .Send(new GetChatMessages.Query(chatId), cancellationToken))
            .OrderBy(m => m.SentAt)
            .ToList();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userChats = await sender.Send(new GetAllChats.Query(userId!), cancellationToken);
        var usersWithChats = userChats.Select(x => int.Parse(x.ReceiverId!));
        var usersWithoutChats = await sender.Send(new GetUsersWithoutChat.Query(int.Parse(userId!), usersWithChats), cancellationToken);

        return View("Index", new ChatsViewModel
        {
            UserId = userId,
            ChatId = chatId,
            UserChats = userChats,
            ReceiverId = receiverId,
            DisplayChatMessages = true,
            ChatMessages = chatMessages,
            UsersWithoutChats = usersWithoutChats,
            ReceiverName = receiverName[..receiverName.IndexOf('@')]
        });
    }
}