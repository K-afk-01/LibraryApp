using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Data;
using LibraryMS.Models;

namespace LibraryMS.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsLoggedIn() => HttpContext.Session.GetString("Username") != null;

        // ===================== BOOK CRUD =====================

        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .OrderBy(b => b.Title)
                .ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null) return NotFound();
            return View(book);
        }

        public IActionResult Create()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"'{book.Title}' kitabı başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns(book.AuthorId, book.CategoryId);
            return View(book);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();
            PopulateDropdowns(book.AuthorId, book.CategoryId);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (id != book.BookId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"'{book.Title}' kitabı başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns(book.AuthorId, book.CategoryId);
            return View(book);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                string title = book.Title;
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"'{title}' kitabı başarıyla silindi.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ===================== CATEGORY CRUD =====================

        public async Task<IActionResult> CategoryIndex()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var categories = await _context.Categories.Include(c => c.Books).ToListAsync();
            return View("~/Views/Category/Index.cshtml", categories);
        }

        public IActionResult CategoryCreate()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            return View("~/Views/Category/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryCreate(Category category)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"'{category.Name}' kategorisi başarıyla eklendi.";
                return RedirectToAction(nameof(CategoryIndex));
            }
            return View("~/Views/Category/Create.cshtml", category);
        }

        public async Task<IActionResult> CategoryEdit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View("~/Views/Category/Edit.cshtml", category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryEdit(int id, Category category)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (id != category.CategoryId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"'{category.Name}' kategorisi başarıyla güncellendi.";
                return RedirectToAction(nameof(CategoryIndex));
            }
            return View("~/Views/Category/Edit.cshtml", category);
        }

        public async Task<IActionResult> CategoryDelete(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var category = await _context.Categories.Include(c => c.Books).FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null) return NotFound();
            return View("~/Views/Category/Delete.cshtml", category);
        }

        [HttpPost, ActionName("CategoryDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CategoryDeleteConfirmed(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                string name = category.Name;
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"'{name}' kategorisi başarıyla silindi.";
            }
            return RedirectToAction(nameof(CategoryIndex));
        }

        // ===================== HELPERS =====================

        private void PopulateDropdowns(int authorId = 0, int categoryId = 0)
        {
            ViewBag.AuthorId = new SelectList(_context.Authors.OrderBy(a => a.LastName)
                .Select(a => new { a.AuthorId, FullName = a.FirstName + " " + a.LastName }),
                "AuthorId", "FullName", authorId);

            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(c => c.Name),
                "CategoryId", "Name", categoryId);
        }
    }
}