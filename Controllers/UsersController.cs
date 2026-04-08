using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    [Authorize]  // ✅ All actions require login
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db) { _db = db; }

        private string CurrentRole =>
            User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "guest";

        public IActionResult Index(string search = "", string filterLetter = "")
        {
            ViewBag.Role = CurrentRole;
            ViewBag.Search = search;

            var users = _db.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                users = users.Where(u => u.Name.Contains(search) || u.Email.Contains(search));

            if (!string.IsNullOrEmpty(filterLetter))
                users = users.Where(u => u.Name.StartsWith(filterLetter));

            return View(users.ToList());
        }

        [Authorize(Roles = "admin")]  // ✅ Admin only
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
                TempData["Error"] = "Unable to add user. Please complete all required fields.";
                return RedirectToAction("Index");
            }

            if (_db.Users.Any(u => u.Email == model.Email))
            {
                TempData["Error"] = "Email already exists.";
                return RedirectToAction("Index");
            }

            _db.Users.Add(model);
            _db.SaveChanges();
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
                _db.Users.Remove(user);
                _db.SaveChanges();
                TempData["Success"] = "User deleted.";
            }
            return RedirectToAction("Index");
        }
    }
}