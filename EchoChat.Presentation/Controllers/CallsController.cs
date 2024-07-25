using Microsoft.AspNetCore.Mvc;

namespace EchoChat.Controllers;

public class CallsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}