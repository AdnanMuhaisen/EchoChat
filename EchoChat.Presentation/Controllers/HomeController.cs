using Microsoft.AspNetCore.Mvc;

namespace EchoChat.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}