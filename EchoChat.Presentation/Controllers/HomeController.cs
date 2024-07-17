using Microsoft.AspNetCore.Mvc;

namespace EchoChat.Presentation.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
