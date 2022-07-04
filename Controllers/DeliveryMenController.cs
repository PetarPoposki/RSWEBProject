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
    public class DeliveryMenController : Controller
    {
        private readonly RSWEBProjectContext _context;
        private UserManager<RSWEBProjectUser> _userManager;
        public DeliveryMenController(RSWEBProjectContext context, UserManager<RSWEBProjectUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        // GET: DeliveryMen
        public async Task<IActionResult> Index(string fullName)
        {
            IQueryable<DeliveryMan> deliveryMen = _context.DeliveryMan.AsQueryable();
            if (!string.IsNullOrEmpty(fullName))
            {
                if (fullName.Contains(" "))
                {
                    string[] names = fullName.Split(" ");
                    deliveryMen = deliveryMen.Where(x => x.FirstName.Contains(names[0]) || x.LastName.Contains(names[1]) ||
                    x.FirstName.Contains(names[1]) || x.LastName.Contains(names[0])); ;
                }
                else
                {
                    deliveryMen = deliveryMen.Where(x => x.FirstName.Contains(fullName) || x.LastName.Contains(fullName));
                }
            }
            
            deliveryMen = deliveryMen.Include(m => m.Restaurants).ThenInclude(m => m.Orders).ThenInclude(m => m.Client); 
            var movieGenreVM = new DeliveryManQuery
            {

                DeliveryMen = await deliveryMen.ToListAsync()
            };
            return View(movieGenreVM);
        }
        // GET: DeliveryMen/Details/5
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
            var deliveryMan = await _context.DeliveryMan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliveryMan == null)
            {
                return NotFound();
            }

            DeliveryManPicture viewmodel = new DeliveryManPicture
            {
                deliveryMan = deliveryMan,
                ProfilePictureName = deliveryMan.ProfilePicture
            };

            return View(viewmodel);
        }
        [Authorize(Roles = "Admin")]
        // GET: DeliveryMen/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        // POST: DeliveryMen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,HireDate,ProfilePicture")] DeliveryMan deliveryMan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliveryMan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deliveryMan);
        }
        [Authorize(Roles = "Admin")]
        // GET: DeliveryMen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DeliveryMan == null)
            {
                return NotFound();
            }

            var deliveryMan = await _context.DeliveryMan.FindAsync(id);
            if (deliveryMan == null)
            {
                return NotFound();
            }
            return View(deliveryMan);
        }
        [Authorize(Roles = "Admin")]
        // POST: DeliveryMen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,HireDate,ProfilePicture")] DeliveryMan deliveryMan)
        {
            if (id != deliveryMan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryMan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryManExists(deliveryMan.Id))
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
            return View(deliveryMan);
        }
        [Authorize(Roles = "Admin")]
        // GET: DeliveryMen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DeliveryMan == null)
            {
                return NotFound();
            }

            var deliveryMan = await _context.DeliveryMan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deliveryMan == null)
            {
                return NotFound();
            }

            return View(deliveryMan);
        }
        [Authorize(Roles = "Admin")]
        // POST: DeliveryMen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DeliveryMan == null)
            {
                return Problem("Entity set 'RSWEBProjectContext.DeliveryMan'  is null.");
            }
            var deliveryMan = await _context.DeliveryMan.FindAsync(id);
            if (deliveryMan != null)
            {
                _context.DeliveryMan.Remove(deliveryMan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeliveryManExists(int id)
        {
          return (_context.DeliveryMan?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [Authorize(Roles = "Delivery Man")]
        // GET: DeliveryMen/EditPicture/5
        public async Task<IActionResult> EditPicture(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryMan = _context.DeliveryMan.Where(x => x.Id == id).First();
            if (deliveryMan == null)
            {
                return NotFound();
            }

            DeliveryManPicture viewmodel = new DeliveryManPicture
            {
                deliveryMan = deliveryMan,
                ProfilePictureName = deliveryMan.ProfilePicture
            };
            
            return View(viewmodel);
        }
        [Authorize(Roles = "Delivery Man")]
        // POST: DeliveryMen/EditPicture/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPicture(long id, DeliveryManPicture viewmodel)
        {
            if (id != viewmodel.deliveryMan.Id)
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
                        viewmodel.deliveryMan.ProfilePicture = uniqueFileName;
                    }
                    else
                    {
                        viewmodel.deliveryMan.ProfilePicture = viewmodel.ProfilePictureName;
                    }

                    _context.Update(viewmodel.deliveryMan);
                   
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryManExists(viewmodel.deliveryMan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = viewmodel.deliveryMan.Id });
            }
            return View(viewmodel);
        }
        private string UploadedFile(DeliveryManPicture viewmodel)
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
