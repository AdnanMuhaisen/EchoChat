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
    public async Task<IActionResult> Index(ChatsViewModel chatsViewModel, CancellationToken cancellationToken)
    {
        var targetViewModel = new ChatsViewModel();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userChats = await sender.Send(new GetAllChats.Query(userId!), cancellationToken);
        var usersWithChats = userChats.Select(x => int.Parse(x.ReceiverId!));
        var usersWithoutChats = await sender.Send(new GetUsers.Query(int.Parse(userId!), usersWithChats), cancellationToken);
        if (chatsViewModel.DisplayChatMessages is not null and true)
        {
            targetViewModel.DisplayChatMessages = true;
            targetViewModel.ChatId = chatsViewModel.ChatId;
            targetViewModel.ReceiverId = chatsViewModel.ReceiverId;
            targetViewModel.ReceiverName = chatsViewModel.ReceiverName;
            targetViewModel.ChatMessages = chatsViewModel.ChatMessages;
        }

        targetViewModel.UserChats = userChats;
        targetViewModel.UsersWithoutChats = usersWithoutChats;

        return View(targetViewModel);
    }

    [HttpGet("new-chat/{receiverId}/{receiverName}")]
    public async Task<IActionResult> Post([FromRoute] string receiverId, [FromRoute] string receiverName, CancellationToken cancellationToken)
    {
        await sender.Send(new CreateChat.Command(User!.FindFirstValue(ClaimTypes.NameIdentifier), receiverId, receiverName), cancellationToken);

        return RedirectToAction("Index");
    }

    [HttpGet("{id}/{receiverId}")]
    public async Task<IActionResult> GetChatMessages([FromRoute(Name = "id")] string chatId, [FromRoute] string receiverId, string receiverName, CancellationToken cancellationToken)
    {
        List<MessageDto> chatMessages = await sender.Send(
            new GetChatMessages.Query(chatId, User.FindFirstValue(ClaimTypes.NameIdentifier), receiverId),
            cancellationToken);

        return RedirectToAction("Index", new ChatsViewModel
        {
            ChatId = chatId,
            ReceiverId = receiverId,
            DisplayChatMessages = true,
            ChatMessages = chatMessages,
            ReceiverName = receiverName
        });
    }
}