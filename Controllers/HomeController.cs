using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Data;

namespace SchoolApp.Controllers
{
    [Authorize]  
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db) { _db = db; }

        [AllowAnonymous]
        public IActionResult Landing()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index");

            return View(new SchoolApp.Models.LoginModel());
        }

        public IActionResult Index()
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "guest";
            ViewBag.Role = role;
            ViewBag.UserName = User.Identity?.Name;
            ViewBag.UserCount = _db.Users.Count();
            ViewBag.TxCount = _db.Transactions.Count();
            ViewBag.TotalAmount = _db.Transactions.Sum(t => (decimal?)t.Amount) ?? 0;
            return View();
        }
    }
}