using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    [Authorize]  //  All actions require login
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db) { _db = db; }

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

        public IActionResult Index(string search = "", string roleFilter = "", int page = 1, int pageSize = 8)
        {
            ViewBag.Role = CurrentRole;
            ViewBag.Search = search;
            
            ViewBag.RoleFilter = roleFilter;

            var users = _db.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                users = users.Where(u => u.Name.Contains(search) || u.Email.Contains(search));

            if (!string.IsNullOrEmpty(roleFilter))
                users = users.Where(u => u.Role == roleFilter);

            var totalCount = users.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            if (totalPages == 0) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;

            var pagedUsers = users
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(pagedUsers);
        }

        [Authorize(Roles = "admin")]  //  Admin only
        public IActionResult Create()
        {
            ViewBag.Role = CurrentRole;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult Create(UserModel model)
        {
            ViewBag.Role = CurrentRole;
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Unable to add user. Please check your inputs.";
                return RedirectToAction("Index", new { openAddUser = 1 });
            }

            if (_db.Users.Any(u => u.Email == model.Email))
            {
                TempData["Error"] = "Email already exists.";
                return RedirectToAction("Index", new { openAddUser = 1 });
            }

            _db.Users.Add(model);
            _db.SaveChanges();
            LogActivity("User Created", $"User '{model.Name}' ({model.Email}) was created.");
            TempData["Success"] = $"User '{model.Name}' added!";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit(int id)
        {
            ViewBag.Role = CurrentRole;
            var user = _db.Users.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(UserModel model)
        {
            ViewBag.Role = CurrentRole;
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Password");

            if (!ModelState.IsValid) return View(model);

            var existing = _db.Users.Find(model.Id);
            if (existing == null) return NotFound();

            existing.Name = model.Name;
            existing.Email = model.Email;
            existing.Age = model.Age;
            existing.Role = model.Role;

            _db.SaveChanges();
            LogActivity("User Edited", $"User '{existing.Name}' (ID: {existing.Id}) was updated.");
            TempData["Success"] = "User updated!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            var user = _db.Users.Find(id);
            if (user != null)
            {
                var deletedName = user.Name;
                var deletedId = user.Id;
                _db.Users.Remove(user);
                _db.SaveChanges();
                LogActivity("User Deleted", $"User '{deletedName}' (ID: {deletedId}) was deleted.");
                TempData["Success"] = "User deleted.";
            }
            return RedirectToAction("Index");
        }
    }
}