using Microsoft.AspNetCore.Mvc;
using SchoolApp.Data;

namespace SchoolApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var role = Request.Query["role"].ToString();
            if (string.IsNullOrEmpty(role)) role = "guest";
            ViewBag.Role = role;
            ViewBag.UserCount = _db.Users.Count();
            ViewBag.TxCount = _db.Transactions.Count();
            ViewBag.TotalAmount = _db.Transactions.Sum(t => (decimal?)t.Amount) ?? 0;
            return View();
        }
    }
}