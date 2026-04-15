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

        private string CurrentUserName => User.Identity?.Name ?? "Unknown";

        private void LogActivity(string action, string details)
        {
            _db.ActivityLogs.Add(new ActivityLogModel
            {
                Action = action,
                Details = details,
                PerformedBy = CurrentUserName,
                CreatedAt = DateTime.Now
            });
            _db.SaveChanges();
        }

        public IActionResult Index(string search = "", string category = "", string dateFrom = "", int page = 1, int pageSize = 8)
        {
            ViewBag.Role = CurrentRole;
            ViewBag.Search = search;
            ViewBag.Category = category;
            ViewBag.DateFrom = dateFrom;

            var txs = _db.Transactions.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                txs = txs.Where(t => t.Description.Contains(search));
            if (!string.IsNullOrEmpty(category))
                txs = txs.Where(t => t.Category == category);
            if (DateTime.TryParse(dateFrom, out var dt))
                txs = txs.Where(t => t.Date >= dt);

            var totalCount = txs.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            var pagedTxs = txs
                .OrderByDescending(t => t.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(pagedTxs);
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
            LogActivity("Transaction Created", $"Transaction #{model.TransactionId} ({model.Category}) amount ₱{model.Amount:N2} was created.");
            TempData["Success"] = "Transaction added!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            var tx = _db.Transactions.Find(id);
            if (tx != null)
            {
                var details = $"Transaction #{tx.TransactionId} ({tx.Category}) amount ₱{tx.Amount:N2} was deleted.";
                _db.Transactions.Remove(tx);
                _db.SaveChanges();
                LogActivity("Transaction Deleted", details);
                TempData["Success"] = "Transaction deleted.";
            }

            return RedirectToAction("Index");
        }
    }
}