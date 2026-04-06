using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;

namespace SchoolApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        // LIST with Search + Filter
        public IActionResult Index(string role = "guest", string search = "", string filterLetter = "")
        {
            ViewBag.Role = role;
            ViewBag.Search = search;
            ViewBag.FilterLetter = filterLetter;

            var users = _db.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                users = users.Where(u => u.Name.Contains(search) || u.Email.Contains(search));

            if (!string.IsNullOrEmpty(filterLetter))
                users = users.Where(u => u.Name.StartsWith(filterLetter));

            return View(users.ToList());
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
        public IActionResult Create(UserModel model, string role = "guest")
        {
            ViewBag.Role = role;

            if (!ModelState.IsValid)
                return View(model);

            _db.Users.Add(model);
            _db.SaveChanges();  // ✅ Saves to SQLite

            TempData["Success"] = $"User '{model.Name}' added successfully!";
            return RedirectToAction("Index", new { role });
        }

        // EDIT (GET)
        public IActionResult Edit(int id, string role = "guest")
        {
            ViewBag.Role = role;
            var user = _db.Users.Find(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserModel model, string role = "guest")
        {
            ViewBag.Role = role;

            // Remove ConfirmPassword validation on edit (field may be blank)
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Password");

            if (!ModelState.IsValid)
                return View(model);

            var existing = _db.Users.Find(model.Id);
            if (existing == null) return NotFound();

            existing.Name = model.Name;
            existing.Email = model.Email;
            existing.Age = model.Age;

            _db.SaveChanges();  // ✅ Updates in SQLite

            TempData["Success"] = "User updated successfully!";
            return RedirectToAction("Index", new { role });
        }

        // DELETE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, string role = "guest")
        {
            var user = _db.Users.Find(id);
            if (user != null)
            {
                _db.Users.Remove(user);
                _db.SaveChanges();  // ✅ Deletes from SQLite
                TempData["Success"] = "User deleted.";
            }
            return RedirectToAction("Index", new { role });
        }
    }
}