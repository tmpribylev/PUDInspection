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
    public class PUDsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PUDsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PUDs
        public async Task<IActionResult> Index()
        {
            return View(await _context.PUDs.ToListAsync());
        }

        // GET: PUDs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pUD = await _context.PUDs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pUD == null)
            {
                return NotFound();
            }

            return View(pUD);
        }

        // GET: PUDs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PUDs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LinkId,Details")] PUD pUD)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pUD);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pUD);
        }

        // GET: PUDs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pUD = await _context.PUDs.FindAsync(id);
            if (pUD == null)
            {
                return NotFound();
            }
            return View(pUD);
        }

        // POST: PUDs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LinkId,Details")] PUD pUD)
        {
            if (id != pUD.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pUD);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PUDExists(pUD.Id))
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
            return View(pUD);
        }

        // GET: PUDs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pUD = await _context.PUDs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pUD == null)
            {
                return NotFound();
            }

            return View(pUD);
        }

        // POST: PUDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pUD = await _context.PUDs.FindAsync(id);
            _context.PUDs.Remove(pUD);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PUDExists(int id)
        {
            return _context.PUDs.Any(e => e.Id == id);
        }
    }
}
