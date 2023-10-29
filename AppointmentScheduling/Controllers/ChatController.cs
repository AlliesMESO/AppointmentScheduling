using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduling.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }

    }
}
