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
    [Authorize(Roles = "validator")]
    public class ValidatorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public ValidatorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public ActionResult Index()
        {
            return RedirectToAction("ValidationList");
        }

        public async Task<IActionResult> ValidationList()
        {
            var user_helper = await userManager.GetUserAsync(HttpContext.User);

            var all_user = await _context.Users.Include(i => i.Validations).ToListAsync();
            var user = all_user.Find(i => i.Id == user_helper.Id);

            if (user.Validations == null)
            {
                user.Validations = new List<Validation>();
            }

            List<ValidatorListViewModel> model = new List<ValidatorListViewModel>();
            foreach (var validation in user.Validations)
            {
                if (!validation.Closed)
                {
                    ValidatorListViewModel viewModel = new ValidatorListViewModel()
                    {
                        ValidationId = validation.Id,
                        ValidationName = validation.Name,
                        StartDate = validation.StartDate,
                        EndDate = validation.EndDate,
                        IterationNumber = validation.IterationNumber,
                        CurrentIteration = validation.CurrentIteration,
                        Hunt = validation.Hunt,
                        Opened = validation.Started
                    };

                    model.Add(viewModel);
                }
            }

            TempData["userId"] = user.Id;
            return View(model);
        }


        // GET: Validator/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var validation = await _context.Validations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (validation == null)
            {
                return NotFound();
            }

            return View(validation);
        }

        // GET: Validator/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Validator/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,IterationNumber,CurrentIteration,Hunt,Started,Closed")] Validation validation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(validation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(validation);
        }

        // GET: Validator/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var validation = await _context.Validations.FindAsync(id);
            if (validation == null)
            {
                return NotFound();
            }
            return View(validation);
        }

        // POST: Validator/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,IterationNumber,CurrentIteration,Hunt,Started,Closed")] Validation validation)
        {
            if (id != validation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(validation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ValidationExists(validation.Id))
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
            return View(validation);
        }

        // GET: Validator/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var validation = await _context.Validations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (validation == null)
            {
                return NotFound();
            }

            return View(validation);
        }

        // POST: Validator/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var validation = await _context.Validations.FindAsync(id);
            _context.Validations.Remove(validation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ValidationExists(int id)
        {
            return _context.Validations.Any(e => e.Id == id);
        }
    }
}
