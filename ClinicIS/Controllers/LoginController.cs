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
        public static int CurrentUserId; // Global variable to store the current user's ID

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
                    // Set the global variable to the current user's ID
                    CurrentUserId = foundUser.user_id;

                    // Check the user's role
                    if (foundUser.role == 1) // Assuming role 1 is for a certain role that needs to be redirected
                    {
                        // Redirect to Complaints/Create if the user's role is 1
                        return RedirectToAction("Create", "Complaints");
                    }
                    else
                    {
                        // Redirect to home page for other roles
                        return RedirectToAction("Index", "Home");
                    }
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
