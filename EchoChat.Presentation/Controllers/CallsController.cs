using EchoChat.Features.Users;
using EchoChat.Models.ViewModels.Calls;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EchoChat.Controllers;

[Authorize]
public class CallsController(ISender sender) : Controller
{
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var users = await sender.Send(new GetUsers.Query());
        users.Remove(users.First(u => u.Id == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!)));
        foreach (var user in users)
        {
            user.UserName = user.UserName?[..user.UserName.IndexOf('@')];
        }

        var callsViewModel = new CallsViewModel
        {
            Users = users,
            UserId = userId
        };

        return View(callsViewModel);
    }
}