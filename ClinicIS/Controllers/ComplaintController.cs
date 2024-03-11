using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicIS.Models;
using produkto.DataConnection;


namespace ClinicIS.Controllers
{
    public class ComplaintsController : Controller
    {
   
        private readonly MySqlDbContext _context;

        public ComplaintsController( MySqlDbContext context)
        {
          
            _context = context;
        }


        private async Task<string> GetUsernameForUserId(int? userId)
        {
            if (userId == null)
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.user_id == userId);
            return user?.username;
        }

        public async Task<IActionResult> Index()
        {
            var complaints = await _context.Complaints.ToListAsync();
            ViewBag.UsernameForComplaintId = new Dictionary<int, string>(); // Initialize ViewBag here
            foreach (var complaint in complaints)
            {
                ViewBag.UsernameForComplaintId[complaint.complaint_id] = await GetUsernameForUserId(complaint.user_id);
            }
            return View(complaints);
        }



        // GET: Complaints/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Complaints/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("complaint_id,complaint_text")] Complaint complaint)
        {
            if (ModelState.IsValid)
            {
                complaint.user_id = LoginController.CurrentUserId; // Set the user_id to the currently logged-in user's ID
                complaint.complaint_date = DateTime.Now; // Set the complaint date to the current date and time
                _context.Add(complaint);
                await _context.SaveChangesAsync();

                // Get the user's role
                var currentUser = await _context.Users.FindAsync(LoginController.CurrentUserId);
                if (currentUser != null && currentUser.role == 1)
                {
                    // Redirect to logout if the user's role is 1
                    return RedirectToAction("Logout", "Login");
                }

                // Redirect to the index action of ComplaintsController if the user's role is not 1
                return RedirectToAction(nameof(Index));
            }

            return View(complaint);
        }


    }
}
