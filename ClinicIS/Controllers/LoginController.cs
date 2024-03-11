using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicIS.Models;
using produkto.DataConnection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ClinicIS.Controllers
{
    public class LoginController : Controller
    {
        private readonly MySqlDbContext _context;

        public LoginController(MySqlDbContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(User user)
        {
            if (ModelState.IsValid)
            {
                // Find the user by username and password
                var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.username == user.username && u.password == user.password);

                if (foundUser != null)
                {
                    // Redirect to home page if the user is found
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // User not found or password incorrect, add model error
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View(user);
                }
            }

            // If model state is invalid, return the view with the model
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
