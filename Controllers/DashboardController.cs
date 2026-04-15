using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Data;

namespace SchoolApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;

        public DashboardController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "guest";
            ViewBag.Role = role;
            ViewBag.UserName = User.Identity?.Name;
            ViewBag.UserCount = _db.Users.Count();
            ViewBag.TxCount = _db.Transactions.Count();
            ViewBag.TotalAmount = _db.Transactions.Sum(t => (decimal?)t.Amount) ?? 0;

            var today = DateTime.Today;
            var weekDays = Enumerable.Range(0, 7)
                .Select(i => today.AddDays(-6 + i))
                .ToList();

            var revenueByDay = _db.Transactions
                .Where(t => t.Date.Date >= weekDays.First() && t.Date.Date <= today)
                .GroupBy(t => t.Date.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(x => x.Amount) })
                .ToDictionary(x => x.Date, x => x.Total);

            var weeklyLabels = weekDays.Select(d => d.ToString("ddd"));
            var weeklyData = weekDays.Select(d => revenueByDay.TryGetValue(d, out var total) ? total : 0m);

            ViewBag.WeeklyLabels = System.Text.Json.JsonSerializer.Serialize(weeklyLabels);
            ViewBag.WeeklyData = System.Text.Json.JsonSerializer.Serialize(weeklyData);

            var categoryStats = _db.Transactions
                .GroupBy(t => string.IsNullOrWhiteSpace(t.Category) ? "General" : t.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    Count = g.Count(),
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToList();

            ViewBag.CategoryLabels = System.Text.Json.JsonSerializer.Serialize(categoryStats.Select(x => x.Category));
            ViewBag.CategoryCountData = System.Text.Json.JsonSerializer.Serialize(categoryStats.Select(x => x.Count));
            ViewBag.CategoryAmountData = System.Text.Json.JsonSerializer.Serialize(categoryStats.Select(x => x.TotalAmount));

            return View();
        }
    }
}
