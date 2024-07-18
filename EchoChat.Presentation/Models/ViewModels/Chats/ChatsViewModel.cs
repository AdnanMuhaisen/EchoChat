using EchoChat.Dtos;

namespace EchoChat.Models.ViewModels.Chats;

public class ChatsViewModel
{
    public List<ChatDto> UserChats { get; set; } = [];

    public List<ApplicationUserDto> UsersWithoutChats { get; set; } = [];

    public bool? DisplayChatMessages { get; set; }

    public List<MessageDto> ChatMessages { get; set; } = [];

    public string? ChatId { get; set; }

    public string? ReceiverId { get; set; }

    public string? ReceiverName { get; set; }
}