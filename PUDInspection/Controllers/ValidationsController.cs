using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PUDInspection.Data;
using PUDInspection.Models;

namespace PUDInspection.Controllers
{
    public class ValidationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ValidationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Validations
        public async Task<IActionResult> Index(int? id)
        {
            //return View(await _context.Validations.ToListAsync());

            if (id == null)
            {
                return NotFound();
            }

            var all_valid = await _context.Validations.Include(i => i.Inspection).ToListAsync();
            var validations = from valid in all_valid
                              where valid.Inspection != null && valid.Inspection.Id == id
                              select valid;

            if (validations == null)
            {
                return NotFound();
            }
            TempData["InspSpaceID"] = id;
            return View(validations);
        }

        // GET: Validations/Details/5
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

        // GET: Validations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Validations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,IterationNumber,CurrentIteration,Hunt,Closed")] Validation validation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(validation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(validation);
        }

        // GET: Validations/Edit/5
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

        // POST: Validations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,IterationNumber,CurrentIteration,Hunt,Closed")] Validation validation)
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

        // GET: Validations/Delete/5
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

        // POST: Validations/Delete/5
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
