using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Data;
using LibraryMS.Models;

namespace LibraryMS.Controllers
{
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;

        public AuthorController(AppDbContext context)
        {
            _context = context;
        }

        private bool IsLoggedIn() => HttpContext.Session.GetString("Username") != null;

        // ===================== AUTHOR CRUD =====================

        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var authors = await _context.Authors.Include(a => a.Books).OrderBy(a => a.LastName).ToListAsync();
            return View(authors);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var author = await _context.Authors.Include(a => a.Books).ThenInclude(b => b.Category)
                .FirstOrDefaultAsync(a => a.AuthorId == id);
            if (author == null) return NotFound();
            return View(author);
        }

        public IActionResult Create()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (ModelState.IsValid)
            {
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"'{author.FirstName} {author.LastName}' yazarı başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Author author)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (id != author.AuthorId) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(author);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"'{author.FirstName} {author.LastName}' yazarı başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.AuthorId == id);
            if (author == null) return NotFound();
            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                string name = $"{author.FirstName} {author.LastName}";
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"'{name}' yazarı başarıyla silindi.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ===================== ORDER CRUD =====================

        public async Task<IActionResult> OrderIndex()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var orders = await _context.Orders.Include(o => o.Book).ThenInclude(b => b!.Author)
                .OrderByDescending(o => o.OrderDate).ToListAsync();
            return View("~/Views/Order/Index.cshtml", orders);
        }

        public IActionResult OrderCreate()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            ViewBag.BookId = new SelectList(
                _context.Books.OrderBy(b => b.Title).Select(b => new { b.BookId, b.Title, b.Price }),
                "BookId", "Title");
            ViewBag.Books = _context.Books.ToDictionary(b => b.BookId, b => b.Price);
            return View("~/Views/Order/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderCreate(Order order)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var book = await _context.Books.FindAsync(order.BookId);
            if (book != null)
            {
                order.TotalAmount = book.Price * order.Quantity;
                order.OrderDate = DateTime.Now;
            }
            ModelState.Remove("TotalAmount");
            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Sipariş başarıyla oluşturuldu. Toplam: {order.TotalAmount:C2}";
                return RedirectToAction(nameof(OrderIndex));
            }
            ViewBag.BookId = new SelectList(_context.Books.OrderBy(b => b.Title), "BookId", "Title", order.BookId);
            return View("~/Views/Order/Create.cshtml", order);
        }

        public async Task<IActionResult> OrderEdit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            ViewBag.BookId = new SelectList(_context.Books.OrderBy(b => b.Title), "BookId", "Title", order.BookId);
            return View("~/Views/Order/Edit.cshtml", order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderEdit(int id, Order order)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (id != order.OrderId) return NotFound();
            var book = await _context.Books.FindAsync(order.BookId);
            if (book != null) order.TotalAmount = book.Price * order.Quantity;
            ModelState.Remove("TotalAmount");
            if (ModelState.IsValid)
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sipariş başarıyla güncellendi.";
                return RedirectToAction(nameof(OrderIndex));
            }
            ViewBag.BookId = new SelectList(_context.Books.OrderBy(b => b.Title), "BookId", "Title", order.BookId);
            return View("~/Views/Order/Edit.cshtml", order);
        }

        public async Task<IActionResult> OrderDelete(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var order = await _context.Orders.Include(o => o.Book).FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null) return NotFound();
            return View("~/Views/Order/Delete.cshtml", order);
        }

        [HttpPost, ActionName("OrderDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderDeleteConfirmed(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Sipariş başarıyla silindi.";
            }
            return RedirectToAction(nameof(OrderIndex));
        }
    }
}