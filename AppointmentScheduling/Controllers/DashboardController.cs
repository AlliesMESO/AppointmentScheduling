using Microsoft.AspNetCore.Mvc;

namespace AppointmentScheduling.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
