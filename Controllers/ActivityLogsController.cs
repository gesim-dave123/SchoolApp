using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Data;

namespace SchoolApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class ActivityLogsController : Controller
    {
        private readonly AppDbContext _db;

        public ActivityLogsController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "guest";
            ViewData["Title"] = "Activity Logs";
            ViewData["Subtitle"] = "Audit trail of admin actions across users and transactions.";

            var logs = _db.ActivityLogs
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            return View(logs);
        }
    }
}
