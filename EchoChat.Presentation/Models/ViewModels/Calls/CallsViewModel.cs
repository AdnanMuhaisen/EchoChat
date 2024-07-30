using EchoChat.Dtos;

namespace EchoChat.Models.ViewModels.Calls;

public class CallsViewModel
{
    public List<ApplicationUserDto> Users { get; set; } = [];

    public string? UserId { get; set; }
}