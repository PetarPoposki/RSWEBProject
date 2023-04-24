using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RSWEBProject.Data;
using RSWEBProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RSWEBProject.Areas.Identity.Data;

namespace RSWEBProject.Controllers
{
    public class OrdersController : Controller
    {
        private readonly RSWEBProjectContext _context;
        private UserManager<RSWEBProjectUser> _userManager;
        public OrdersController(RSWEBProjectContext context, UserManager<RSWEBProjectUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var rSWEBProjectContext = _context.Order.Include(o => o.Client).Include(o => o.Restaurant);
            return View(await rSWEBProjectContext.ToListAsync());
        }
        [Authorize(Roles = "Admin")]
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Client)
                .Include(o => o.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [Authorize(Roles = "Client")]
        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Client.Where(x => x.Id == int.Parse(HttpContext.Session.GetString("UserLoggedIn"))), "Id", "FullName");
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Name");
            
            return View();
        }
        [Authorize(Roles = "Client")]
        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RestaurantId,ClientId,SerialNumber,Price")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return LocalRedirect("/Restaurants/ViewOrders/" + HttpContext.Session.GetString("UserLoggedIn"));
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", order.ClientId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Id", order.RestaurantId);
            var userLoggedInId = HttpContext.Session.GetString("UserLoggedIn");

            // return LocalRedirect("/Restaurants/ViewOrders/" + userLoggedInId);

            // return LocalRedirect(pomosen);
            return LocalRedirect("/Restaurants/ViewOrders/" + HttpContext.Session.GetString("UserLoggedIn"));
            //return View(order);
        }
        [Authorize(Roles = "Client")]
        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", order.ClientId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Id", order.RestaurantId);
            
            return View(order);
        }
        [Authorize(Roles = "Client")]
        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RestaurantId,ClientId,SerialNumber,Price")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", order.ClientId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Id", order.RestaurantId);
            
            
            return View(order);
        }
        [Authorize(Roles = "Admin")]
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Client)
                .Include(o => o.Restaurant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [Authorize(Roles = "Admin")]
        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'RSWEBProjectContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [Authorize(Roles = "Delivery Man")]
        // GET: Orders/EditAsDeliveryMan/5
        public async Task<IActionResult> EditAsDeliveryMan(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            
            // ViewBag.deliveryman = deliveryman;
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", order.ClientId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Id", order.RestaurantId);
            return View(order);
        }
        [Authorize(Roles = "Delivery Man")]
        // POST: Orders/EditAsDeliveryMan/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsDeliveryMan(int id, [Bind("Id,RestaurantId,ClientId,SerialNumber,Price")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", order.ClientId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Id", order.RestaurantId);
            return View(order);
        }
        [Authorize(Roles = "Client")]
        // GET: Orders/EditAsClient/5
        public async Task<IActionResult> EditAsClient(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var userLoggedInId = HttpContext.Session.GetString("UserLoggedIn");
            if (userLoggedInId != order.ClientId.ToString() && userLoggedInId != "Admin")
            {
                return Forbid();
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "FullName", order.ClientId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Name", order.RestaurantId);
            return View(order);
        }
        [Authorize(Roles = "Client")]
        // POST: Orders/EditAsClient/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsClient(int id, [Bind("Id,RestaurantId,ClientId,SerialNumber,Price")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            var userLoggedInId = HttpContext.Session.GetString("UserLoggedIn");
            if (userLoggedInId != order.ClientId.ToString() && userLoggedInId != "Admin")
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return LocalRedirect("/Restaurants/ViewOrders/" + order.ClientId);
               // return RedirectToAction("Index");
            }
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Id", order.ClientId);
            ViewData["RestaurantId"] = new SelectList(_context.Restaurant, "Id", "Id", order.RestaurantId);
            return View(order);
        }


    }
}
