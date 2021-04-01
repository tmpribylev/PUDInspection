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
    public class EmailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Emails
        public async Task<IActionResult> Index()
        {
            return View(await _context.EmailTexts.ToListAsync());
        }

        // GET: Emails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailText = await _context.EmailTexts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emailText == null)
            {
                return NotFound();
            }

            return View(emailText);
        }

        // GET: Emails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Emails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GoodPUDText,StartText,EndText,DescriptionText")] EmailText emailText)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emailText);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emailText);
        }

        // GET: Emails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailText = await _context.EmailTexts.FindAsync(id);
            if (emailText == null)
            {
                return NotFound();
            }
            return View(emailText);
        }

        // POST: Emails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GoodPUDText,StartText,EndText,DescriptionText")] EmailText emailText)
        {
            if (id != emailText.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emailText);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailTextExists(emailText.Id))
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
            return View(emailText);
        }

        // GET: Emails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emailText = await _context.EmailTexts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emailText == null)
            {
                return NotFound();
            }

            return View(emailText);
        }

        // POST: Emails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emailText = await _context.EmailTexts.FindAsync(id);
            _context.EmailTexts.Remove(emailText);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmailTextExists(int id)
        {
            return _context.EmailTexts.Any(e => e.Id == id);
        }
    }
}
