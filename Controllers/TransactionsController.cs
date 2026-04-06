using Microsoft.AspNetCore.Mvc;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly AppDbContext _db;

        public TransactionsController(AppDbContext db)
        {
            _db = db;
        }

        // LIST with Search + Filter
        public IActionResult Index(string role = "guest", string search = "",
                                   string category = "", string dateFrom = "")
        {
            ViewBag.Role = role;
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

        // CREATE (GET)
        public IActionResult Create(string role = "guest")
        {
            ViewBag.Role = role;
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TransactionModel model, string role = "guest")
        {
            ViewBag.Role = role;

            if (!ModelState.IsValid)
                return View(model);

            model.Date = DateTime.Now;
            _db.Transactions.Add(model);
            _db.SaveChanges();  // ✅ Saves to SQLite

            TempData["Success"] = "Transaction added!";
            return RedirectToAction("Index", new { role });
        }
    }
}