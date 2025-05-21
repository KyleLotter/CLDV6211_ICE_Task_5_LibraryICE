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
    public class BookTypeController : Controller
    {
        private readonly LibraryDBContext _context;

        public BookTypeController(LibraryDBContext context)
        {
            _context = context;
        }

        // GET: BookType
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bookings' is null."); // Check if the Bookings table is null (Microsoft, 2025).
            }

            var bookTypes = from b in _context.BookType
                        select b; // Getting all bookings from the database (Microsoft, 2025).

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper(); // Convert search string to uppercase (Microsoft, 2025).
                bookTypes = bookTypes.Where(b =>
                    b.TypeID.ToString() == searchString ||
                    (b.Type != null && b.Type.ToUpper().Contains(searchString))); // Check if the booking ID or event name contains the search string (Microsoft, 2025).
            }

            ViewData["Index"] = searchString;
            return View("Index", await bookTypes.ToListAsync());

        }

        // GET: BookType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookType = await _context.BookType
                .FirstOrDefaultAsync(m => m.TypeID == id);
            if (bookType == null)
            {
                return NotFound();
            }

            return View(bookType);
        }

        // GET: BookType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeID,Type")] BookType bookType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookType);
        }

        // GET: BookType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookType = await _context.BookType.FindAsync(id);
            if (bookType == null)
            {
                return NotFound();
            }
            return View(bookType);
        }

        // POST: BookType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeID,Type")] BookType bookType)
        {
            if (id != bookType.TypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookTypeExists(bookType.TypeID))
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
            return View(bookType);
        }

        // GET: BookType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookType = await _context.BookType
                .FirstOrDefaultAsync(m => m.TypeID == id);
            if (bookType == null)
            {
                return NotFound();
            }

            return View(bookType);
        }

        // POST: BookType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookType = await _context.BookType.FindAsync(id);
            if (bookType != null)
            {
                _context.BookType.Remove(bookType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookTypeExists(int id)
        {
            return _context.BookType.Any(e => e.TypeID == id);
        }
    }
}
