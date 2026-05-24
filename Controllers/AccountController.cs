using Microsoft.AspNetCore.Mvc;
using LibraryMS.Data;

namespace LibraryMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("UserId", user.UserId.ToString());
                HttpContext.Session.SetString("Role", user.Role);
                TempData["Success"] = $"Hoş geldiniz, {user.Username}! Sisteme başarıyla giriş yaptınız.";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        public IActionResult Logout()
        {
            string? username = HttpContext.Session.GetString("Username");
            HttpContext.Session.Clear();
            TempData["Success"] = $"Başarıyla çıkış yapıldı. Güle güle, {username}!";
            return RedirectToAction("Login");
        }
    }
}
