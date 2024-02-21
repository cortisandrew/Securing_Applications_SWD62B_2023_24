using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Securing_Applications_SWD62B_2023_24.Data;
using Securing_Applications_SWD62B_2023_24.Models;

namespace Securing_Applications_SWD62B_2023_24.Controllers
{
    public class AppraisalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppraisalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appraisals
        public async Task<IActionResult> Index()
        {
            return _context.Appraisals != null ?
                        View(await _context.Appraisals.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Appraisals'  is null.");
        }

        // GET: Appraisals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Appraisals == null)
            {
                return NotFound();
            }

            var appraisal = await _context.Appraisals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appraisal == null)
            {
                return NotFound();
            }

            return View(appraisal);
        }

        // GET: Appraisals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appraisals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Comment")] Appraisal appraisal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appraisal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appraisal);
        }

        // GET: Appraisals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Appraisals == null)
            {
                return NotFound();
            }

            var appraisal = await _context.Appraisals.FindAsync(id);
            if (appraisal == null)
            {
                return NotFound();
            }
            return View(appraisal);
        }

        // POST: Appraisals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comment")] Appraisal appraisal)
        {
            if (id != appraisal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appraisal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppraisalExists(appraisal.Id))
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
            return View(appraisal);
        }

        // GET: Appraisals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Appraisals == null)
            {
                return NotFound();
            }

            var appraisal = await _context.Appraisals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appraisal == null)
            {
                return NotFound();
            }

            return View(appraisal);
        }

        // POST: Appraisals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Appraisals == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Appraisals'  is null.");
            }
            var appraisal = await _context.Appraisals.FindAsync(id);
            if (appraisal != null)
            {
                _context.Appraisals.Remove(appraisal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppraisalExists(int id)
        {
            return (_context.Appraisals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
