using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly AppDbContext _db;
        public TransactionsController(AppDbContext db) { _db = db; }

        private string CurrentRole =>
            User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "guest";

        public IActionResult Index(string search = "", string category = "", string dateFrom = "")
        {
            ViewBag.Role = CurrentRole;
            ViewBag.Search = search;

            var txs = _db.Transactions.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                txs = txs.Where(t => t.Description.Contains(search));
            if (!string.IsNullOrEmpty(category))
                txs = txs.Where(t => t.Category == category);
            if (DateTime.TryParse(dateFrom, out var dt))
                txs = txs.Where(t => t.Date >= dt);

            return View(txs.OrderByDescending(t => t.Date).ToList());
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewBag.Role = CurrentRole;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult Create(TransactionModel model)
        {
            ViewBag.Role = CurrentRole;
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Unable to add transaction. Please complete all required fields.";
                return RedirectToAction("Index");
            }

            model.Date = DateTime.Now;
            _db.Transactions.Add(model);
            _db.SaveChanges();
            TempData["Success"] = "Transaction added!";
            return RedirectToAction("Index");
        }
    }
}