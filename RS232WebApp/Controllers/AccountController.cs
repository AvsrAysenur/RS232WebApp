using Microsoft.AspNetCore.Mvc;
using RS232WebApp.Models;
using RS232WebApp.Data;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace RS232WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Şifreyi hashle
                user.Password = HashPassword(user.Password);

                // Kullanıcıyı veritabanına kaydet
                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == model.Username && u.Password == HashPassword(model.Password));
            if (user != null)
            {
                return RedirectToAction("Chat");
            }
            ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre");
            return View(model);
        }

        // Yardımcı metodları static olarak işaretleyin
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
