using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RSWEBProject.Data;
using RSWEBProject.Models;
using RSWEBProject.ViewModels;

namespace RSWEBProject.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly RSWEBProjectContext _context;

        public RestaurantsController(RSWEBProjectContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        // GET: Restaurants
        public async Task<IActionResult> Index(string type, string name, string location)
        {
            IQueryable<Restaurant> restaurants = _context.Restaurant.AsQueryable();
            IQueryable<string> typeQuery = _context.Restaurant.OrderBy(m => m.Type).Select(m => m.Type).Distinct();
            if (!string.IsNullOrEmpty(name))
            {
                restaurants = restaurants.Where(s => s.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(location))
            {
                restaurants = restaurants.Where(x => x.Location == location);
            }
            if (!string.IsNullOrEmpty(location))
            {
                restaurants = restaurants.Where(x => x.Location == location);
            }
            if (!string.IsNullOrEmpty(type))
            {
                restaurants = restaurants.Where(p => p.Type == type);
            }
            restaurants = restaurants.Include(m => m.DeliveryMan)
            .Include(m => m.Orders).ThenInclude(m => m.Client);
            var movieGenreVM = new RestaurantQuery
            {
                Types = new SelectList(await typeQuery.ToListAsync()),
                Restaurants = await restaurants.ToListAsync()
            };
            return View(movieGenreVM);




         
        }
        [Authorize(Roles = "Admin")]
        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Restaurant == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .Include(r => r.DeliveryMan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }
        [Authorize(Roles = "Admin")]
        // GET: Restaurants/Create
        public IActionResult Create()
        {
            ViewData["DeliveryManId"] = new SelectList(_context.DeliveryMan, "Id", "Id");
            return View();
        }
        [Authorize(Roles = "Admin")]
        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Type,DeliveryManId")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeliveryManId"] = new SelectList(_context.DeliveryMan, "Id", "Id", restaurant.DeliveryManId);
            return View(restaurant);
        }
        [Authorize(Roles = "Admin")]
        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Restaurant == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            ViewData["DeliveryManId"] = new SelectList(_context.DeliveryMan, "Id", "Id", restaurant.DeliveryManId);
            return View(restaurant);
        }
        [Authorize(Roles = "Admin")]
        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,Type,DeliveryManId")] Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.Id))
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
            ViewData["DeliveryManId"] = new SelectList(_context.DeliveryMan, "Id", "Id", restaurant.DeliveryManId);
            return View(restaurant);
        }
        [Authorize(Roles = "Admin")]
        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Restaurant == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .Include(r => r.DeliveryMan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }
        [Authorize(Roles = "Admin")]
        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Restaurant == null)
            {
                return Problem("Entity set 'RSWEBProjectContext.Restaurant'  is null.");
            }
            var restaurant = await _context.Restaurant.FindAsync(id);
            if (restaurant != null)
            {
                _context.Restaurant.Remove(restaurant);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
          return (_context.Restaurant?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [Authorize(Roles = "Client")]
        // GET: ViewOrders
        public async Task<IActionResult> ViewOrders(int id)
        {
            var userLoggedInId = HttpContext.Session.GetString("UserLoggedIn");
           // if (userLoggedInId != id.ToString() && userLoggedInId != "Admin")
           // {
             //   return Forbid();
            //}
            var kolokviumContext = _context.Order.Where(x => x.ClientId == int.Parse(HttpContext.Session.GetString("UserLoggedIn"))).Include(m => m.Client).Include(a => a.Restaurant);
            
          return View(await kolokviumContext.ToListAsync());
        }
        [Authorize(Roles = "Delivery Man")]
        // GET: ViewRestaurants
        public async Task<IActionResult> ViewRestaurants(int id, string type, string name, string location)
        {
            var userLoggedInId = HttpContext.Session.GetString("UserLoggedIn");
            if (userLoggedInId != id.ToString() && userLoggedInId != "Admin")
            {
                return Forbid();
            }
            IQueryable<Restaurant> restaurants = _context.Restaurant.AsQueryable();
            restaurants = restaurants.Where(x => x.DeliveryManId == id).Include(m => m.DeliveryMan);
            IQueryable<string> typeQuery = _context.Restaurant.OrderBy(m => m.Type).Select(m => m.Type).Distinct();
            if (!string.IsNullOrEmpty(name))
            {
                restaurants = restaurants.Where(s => s.Name.Contains(name));
            }
            if (!string.IsNullOrEmpty(location))
            {
                restaurants = restaurants.Where(x => x.Location == location);
            }
            if (!string.IsNullOrEmpty(location))
            {
                restaurants = restaurants.Where(x => x.Location == location);
            }
            if (!string.IsNullOrEmpty(type))
            {
                restaurants = restaurants.Where(p => p.Type == type);
            }
            restaurants = restaurants.Include(m => m.DeliveryMan)
            .Include(m => m.Orders).ThenInclude(m => m.Client);
            var movieGenreVM = new RestaurantQuery
            {
                Types = new SelectList(await typeQuery.ToListAsync()),
                Restaurants = await restaurants.ToListAsync()
            };
            return View(movieGenreVM);
        }
    }
}
