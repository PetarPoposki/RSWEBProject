using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RSWEBProject.Areas.Identity.Data;
using RSWEBProject.Data;
using RSWEBProject.Models;
using RSWEBProject.ViewModels;

namespace RSWEBProject.Controllers
{
    public class ClientsController : Controller
    {
        private readonly RSWEBProjectContext _context;
        private UserManager<RSWEBProjectUser> _userManager;
        public ClientsController(RSWEBProjectContext context, UserManager<RSWEBProjectUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        // GET: Clients
        public async Task<IActionResult> Index(string fullName, string address, string phoneNumber, string email)
        {
            IQueryable<Client> clients = _context.Client.AsQueryable();
            if (!string.IsNullOrEmpty(fullName))
            {
                if (fullName.Contains(" "))
                {
                    string[] names = fullName.Split(" ");
                    clients = clients.Where(x => x.FirstName.Contains(names[0]) || x.LastName.Contains(names[1]) ||
                    x.FirstName.Contains(names[1]) || x.LastName.Contains(names[0])); ;
                }
                else
                {
                    clients = clients.Where(x => x.FirstName.Contains(fullName) || x.LastName.Contains(fullName));
                }
            }
            if (!string.IsNullOrEmpty(address))
            {
                clients = clients.Where(x => x.Address == address);
            }
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                clients = clients.Where(x => x.PhoneNumber == phoneNumber);
            }
            if (!string.IsNullOrEmpty(email))
            {
                clients = clients.Where(p => p.Email == email);
            }
            clients = clients.Include(m => m.Orders).ThenInclude(m => m.Restaurant);
            var movieGenreVM = new ClientQuery
            {
                
                Clients = await clients.ToListAsync()
            };
            return View(movieGenreVM);




           
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userLoggedInId = HttpContext.Session.GetString("UserLoggedIn");
            if (userLoggedInId != id.ToString() && userLoggedInId != "Admin")
            {
                return Forbid();
            }
            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            ClientPicture viewmodel = new ClientPicture
            {
                client = client,
                ProfilePictureName = client.ProfilePicture
            };

            return View(viewmodel);




            
        }
        [Authorize(Roles = "Admin")]
        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PhoneNumber,Address,Email,ProfilePicture")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }
        [Authorize(Roles = "Admin")]
        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }
        [Authorize(Roles = "Admin")]
        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PhoneNumber,Address,Email,ProfilePicture")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }
        [Authorize(Roles = "Admin")]
        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }
        [Authorize(Roles = "Admin")]
        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Client == null)
            {
                return Problem("Entity set 'RSWEBProjectContext.Client'  is null.");
            }
            var client = await _context.Client.FindAsync(id);
            if (client != null)
            {
                _context.Client.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return (_context.Client?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [Authorize(Roles = "Delivery Man")]
        // GET: ViewOrders
        public async Task<IActionResult> ViewOrders(int id)
        {
            var kolokviumContext = _context.Order.Where(x => x.RestaurantId == id).Include(m => m.Restaurant).Include(a => a.Client);
            return View(await kolokviumContext.ToListAsync());
        }

        [Authorize(Roles = "Client")]
        // GET: Clients/EditPicture/5
        public async Task<IActionResult> EditPicture(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _context.Client.Where(x => x.Id == id).Include(x => x.Orders).First();
            if (client == null)
            {
                return NotFound();
            }

            var restaurants = _context.Restaurant.AsEnumerable();
            restaurants = restaurants.OrderBy(s => s.Name);

            ClientPicture viewmodel = new ClientPicture
            {
                client = client,
                ProfilePictureName = client.ProfilePicture
            };

            return View(viewmodel);
        }
        [Authorize(Roles = "Client")]
        // POST: Clients/EditPicture/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPicture(long id, ClientPicture viewmodel)
        {
            if (id != viewmodel.client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (viewmodel.ProfilePictureFile != null)
                    {
                        string uniqueFileName = UploadedFile(viewmodel);
                        viewmodel.client.ProfilePicture = uniqueFileName;
                    }
                    else
                    {
                        viewmodel.client.ProfilePicture = viewmodel.ProfilePictureName;
                    }

                    _context.Update(viewmodel.client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(viewmodel.client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = viewmodel.client.Id });
            }
            return View(viewmodel);
        }

        private string UploadedFile(ClientPicture viewmodel)
        {
            string uniqueFileName = null;

            if (viewmodel.ProfilePictureFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profilePictures");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewmodel.ProfilePictureFile.FileName);
                string fileNameWithPath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    viewmodel.ProfilePictureFile.CopyTo(stream);
                }
            }
            return uniqueFileName;
        }

    }
}
