using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolApp.Controllers
{
    [Authorize]  
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Landing()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Dashboard");

            return View(new SchoolApp.Models.LoginModel());
        }
    }
}