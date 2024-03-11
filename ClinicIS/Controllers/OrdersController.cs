using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicIS.Models;
using produkto.DataConnection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicIS.Controllers
{
    public class OrdersController : Controller
    {
        private readonly MySqlDbContext _context;

        public OrdersController(MySqlDbContext context)
        {
            _context = context;
        }
        // GET: Orders/Index
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.ToListAsync();

            // Initialize dictionaries to store user and item names
            ViewBag.UserNameForOrder = new Dictionary<int, string>();
            ViewBag.ItemNameForOrder = new Dictionary<int, string>();

            foreach (var order in orders)
            {
                ViewBag.UserNameForOrder[order.order_id] = GetUserFullName(order.user_id ?? 0); // Provide default value if user_id is null
                ViewBag.ItemNameForOrder[order.order_id] = GetItemName(order.item_id ?? 0); // Provide default value if item_id is null
            }

            return View(orders);
        }

        private string GetUserFullName(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.user_id == userId);
            return user != null ? user.full_name : "Unknown";
        }

        private string GetItemName(int itemId)
        {
            var item = _context.Inventory_Items.FirstOrDefault(i => i.item_id == itemId);
            return item != null ? item.item_name : "Unknown";
        }



        // GET: Orders/Create
        public IActionResult Create()
        {
            // Retrieve the list of available users from the database
            var availableUsers = _context.Users.ToList();

            // Create a select list for the available users
            ViewBag.AvailableUsers = new SelectList(availableUsers, "user_id", "username");

            // Retrieve the list of available inventory items from the database
            var availableItems = _context.Inventory_Items.ToList();

            // Create a select list for the available inventory items
            ViewBag.AvailableItems = new SelectList(availableItems, "item_id", "item_name");

            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("user_id,order_date,item_id,quantity")] Order order)
        {
            if (ModelState.IsValid)
            {
                // Set the order date to the current date and time
                order.order_date = DateTime.Now;

                // Add the order to the context
                _context.Add(order);

                // Update the quantity of the corresponding item in inventory_items
                var item = await _context.Inventory_Items.FindAsync(order.item_id);
                if (item != null)
                {
                    // Deduct the quantity from the inventory
                    item.quantity -= order.quantity;

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Handle the case where the item is not found
                    ModelState.AddModelError(string.Empty, "Item not found in inventory.");
                    return View(order);
                }

                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, return the create view with the model
            return View(order);
        }


        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("order_id,user_id,order_date,item_id,quantity")] Order order)
        {
            if (id != order.order_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the original order from the database
                    var originalOrder = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.order_id == id);

                    // Calculate the difference in quantity
                    int quantityDifference = order.quantity - originalOrder.quantity;

                    // Update the order in the context
                    _context.Update(order);

                    // Update the quantity of the corresponding item in inventory_items
                    var item = await _context.Inventory_Items.FindAsync(order.item_id);
                    if (item != null)
                    {
                        // Update the quantity in inventory
                        item.quantity += quantityDifference;

                        // Save changes to the database
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Handle the case where the item is not found
                        ModelState.AddModelError(string.Empty, "Item not found in inventory.");
                        return View(order);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.order_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.order_id == id);
        }


    }
}
