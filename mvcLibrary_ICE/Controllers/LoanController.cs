using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvcLibrary_ICE.Models;

namespace mvcLibrary_ICE.Controllers
{
    public class LoanController : Controller
    {
        private readonly LibraryDBContext _context;

        public LoanController(LibraryDBContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Loan == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bookings' is null."); // Check if the Bookings table is null (Microsoft, 2025).
            }

            var loans = from b in _context.Loan
                        .Include(l => l.Book)
                        select b; // Getting all bookings from the database (Microsoft, 2025).

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper(); // Convert search string to uppercase (Microsoft, 2025).
                loans = loans.Where(b =>
                    b.LoanID.ToString() == searchString ||
                    (b.Book.Title != null && b.Book.Title.ToUpper().Contains(searchString))); // Check if the booking ID or event name contains the search string (Microsoft, 2025).
            }

            ViewData["Index"] = searchString;
            return View("Index", await loans.ToListAsync());

        }

        // GET: Loan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan
                .Include(l => l.Book)
                .FirstOrDefaultAsync(m => m.LoanID == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loan/Create
        public IActionResult Create()
        {
            ViewData["BookID"] = new SelectList(_context.Book, "BookID", "BookID");
            return View();
        }

        // POST: Loan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LoanID,LoanDate,BookID")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookID"] = new SelectList(_context.Book, "BookID", "BookID", loan.BookID);
            return View(loan);
        }

        // GET: Loan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewData["BookID"] = new SelectList(_context.Book, "BookID", "BookID", loan.BookID);
            return View(loan);
        }

        // POST: Loan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LoanID,LoanDate,BookID")] Loan loan)
        {
            if (id != loan.LoanID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.LoanID))
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
            ViewData["BookID"] = new SelectList(_context.Book, "BookID", "BookID", loan.BookID);
            return View(loan);
        }

        // GET: Loan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loan
                .Include(l => l.Book)
                .FirstOrDefaultAsync(m => m.LoanID == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loan.FindAsync(id);
            if (loan != null)
            {
                _context.Loan.Remove(loan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loan.Any(e => e.LoanID == id);
        }
    }
}
