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
    public class CriteriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CriteriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Criteria
        public async Task<IActionResult> Index()
        {
            return View(await _context.Criterias.ToListAsync());
        }

        // GET: Criteria/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criteria = await _context.Criterias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (criteria == null)
            {
                return NotFound();
            }

            return View(criteria);
        }

        // GET: Criteria/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Criteria/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,MaxValue,AutoCheck,AutoCheckFormula,Important,Deleted")] Criteria criteria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(criteria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(criteria);
        }

        // GET: Criteria/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criteria = await _context.Criterias.FindAsync(id);
            if (criteria == null)
            {
                return NotFound();
            }
            return View(criteria);
        }

        // POST: Criteria/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,MaxValue,AutoCheck,AutoCheckFormula,Important,Deleted")] Criteria criteria)
        {
            if (id != criteria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(criteria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CriteriaExists(criteria.Id))
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
            return View(criteria);
        }

        // GET: Criteria/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var criteria = await _context.Criterias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (criteria == null)
            {
                return NotFound();
            }

            return View(criteria);
        }

        // POST: Criteria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var criteria = await _context.Criterias.FindAsync(id);
            _context.Criterias.Remove(criteria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CriteriaExists(int id)
        {
            return _context.Criterias.Any(e => e.Id == id);
        }
    }
}
