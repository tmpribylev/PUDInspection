using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PUDInspection.Data;
using PUDInspection.Models;
using PUDInspection.ViewModels;

namespace PUDInspection.Controllers
{
    [Authorize(Roles = "admin,moderator")]
    public class InspectionSpacesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public InspectionSpacesController(ApplicationDbContext context,
                                          UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }
        public ActionResult Index()
        {
            return RedirectToAction("SpaceList");
        }

        // GET: InspectionSpaces
        public async Task<IActionResult> SpaceList()
        {
            if (User.IsInRole("admin"))
            {
                return View(await _context.InspectionSpaces.ToListAsync());
            }
            else
            {
                var user_helper = await userManager.GetUserAsync(HttpContext.User);

                var all_user = await _context.Users.Include(i => i.InspectionSpaces).ToListAsync();
                var user = all_user.Find(i => i.Id == user_helper.Id);

                if (user.InspectionSpaces == null)
                {
                    user.InspectionSpaces = new List<InspectionSpace>();
                }

                return View(user.InspectionSpaces);
            }
        }

        // GET: InspectionSpaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var all_space = await _context.InspectionSpaces.Include(i => i.UserList).Include(i => i.InspectionList).ToListAsync();
            var inspectionSpace = all_space.Find(i => i.Id == id);

            if (inspectionSpace == null)
            {
                return NotFound();
            }

            return View(inspectionSpace);
        }

        // GET: InspectionSpaces/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InspectionSpaces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Closed")] InspectionSpace inspectionSpace)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inspectionSpace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inspectionSpace);
        }

        // GET: InspectionSpaces/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditInspectionSpace(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var all_space = await _context.InspectionSpaces.Include(i => i.UserList).ToListAsync();
            var inspectionSpace = all_space.Find(i => i.Id == id);

            if (inspectionSpace == null)
            {
                return NotFound();
            }

            return View(inspectionSpace);
        }

        // POST: InspectionSpaces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditInspectionSpace(int id, [Bind("Id,Name,Closed")] InspectionSpace inspectionSpace)
        {
            if (id != inspectionSpace.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inspectionSpace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InspectionSpaceExists(inspectionSpace.Id))
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
            return View(inspectionSpace);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditUsersInSpace(int spaceId)
        {
            ViewBag.spaceId = spaceId;

            var all_space = await _context.InspectionSpaces.Include(i => i.UserList).ToListAsync();
            var inspectionSpace = all_space.Find(i => i.Id == spaceId);

            if (inspectionSpace == null)
            {
                ViewBag.ErrorMessage = $"Пространства с ID = {spaceId} не найдены";
                return View("NotFound");
            }

            if (inspectionSpace.UserList == null)
            {
                inspectionSpace.UserList = new List<ApplicationUser>();
            }

            var model = new List<UsersInspectionSpaceViewModel>();

            foreach (var user in await userManager.Users.ToListAsync())
            {
                if (await userManager.IsInRoleAsync(user, "moderator"))
                {
                    var usersInspectionSpaceViewModel = new UsersInspectionSpaceViewModel
                    {
                        UserId = user.Id,
                        UserName = user.RealName
                    };

                    if (inspectionSpace.UserList.Contains(user))
                    {
                        usersInspectionSpaceViewModel.IsSelected = true;
                    }
                    else
                    {
                        usersInspectionSpaceViewModel.IsSelected = false;
                    }

                    model.Add(usersInspectionSpaceViewModel);
                }
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> EditUsersInSpace(List<UsersInspectionSpaceViewModel> model, string spaceId)
        {
            int id = Convert.ToInt32(spaceId);

            var all_space = await _context.InspectionSpaces.Include(i => i.UserList).ToListAsync();
            var inspectionSpace = all_space.Find(i => i.Id == id);

            if (inspectionSpace == null)
            {
                ViewBag.ErrorMessage = $"Пространства с ID = {id} не найдены";
                return View("NotFound");
            }

            if (inspectionSpace.UserList == null)
            {
                inspectionSpace.UserList = new List<ApplicationUser>();
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                if (model[i].IsSelected && !(inspectionSpace.UserList.Contains(user)))
                {
                    inspectionSpace.UserList.Add(user);
                }
                else if (!model[i].IsSelected && inspectionSpace.UserList.Contains(user))
                {
                    inspectionSpace.UserList.Remove(user);
                }
                else
                {
                    continue;
                }
            }

            try
            {
                _context.Update(inspectionSpace);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InspectionSpaceExists(inspectionSpace.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("EditInspectionSpace", new { Id = id });
        }

        // GET: InspectionSpaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inspectionSpace = await _context.InspectionSpaces
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(inspectionSpace);
        }

        // POST: InspectionSpaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inspectionSpace = await _context.InspectionSpaces.FindAsync(id);
            _context.InspectionSpaces.Remove(inspectionSpace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InspectionSpaceExists(int id)
        {
            return _context.InspectionSpaces.Any(e => e.Id == id);
        }

        public ActionResult GoToInspections(int? id)
        {
            return RedirectToAction("Index", "Inspections", new { id = id });
        }
    }
}
