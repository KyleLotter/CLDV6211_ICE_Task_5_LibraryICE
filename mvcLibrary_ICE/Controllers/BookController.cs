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
    public class BookController : Controller
    {
        private readonly LibraryDBContext _context;

        public BookController(LibraryDBContext context)
        {
            _context = context;
        }

        // GET: Book
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bookings' is null."); // Check if the Bookings table is null (Microsoft, 2025).
            }

            var books = from b in _context.Book
                           select b; // Getting all bookings from the database (Microsoft, 2025).

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper(); // Convert search string to uppercase (Microsoft, 2025).
                books = books.Where(b =>
                    b.BookID.ToString() == searchString ||
                    (b.Title != null && b.Title.ToUpper().Contains(searchString))); // Check if the booking ID or event name contains the search string (Microsoft, 2025).
            }

            ViewData["Index"] = searchString;
            return View("Index", await books.ToListAsync());

        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Type)
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            ViewData["TypeID"] = new SelectList(_context.BookType, "TypeID", "TypeID");
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookID,Title,Author,ISBN,TypeID")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeID"] = new SelectList(_context.BookType, "TypeID", "TypeID", book.TypeID);
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["TypeID"] = new SelectList(_context.BookType, "TypeID", "TypeID", book.TypeID);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookID,Title,Author,ISBN,TypeID")] Book book)
        {
            if (id != book.BookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID))
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
            ViewData["TypeID"] = new SelectList(_context.BookType, "TypeID", "TypeID", book.TypeID);
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Type)
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.BookID == id);
        }
    }
}
