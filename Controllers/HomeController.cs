using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Data;
using LibraryMS.Models;

namespace LibraryMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private bool IsLoggedIn() => HttpContext.Session.GetString("Username") != null;

        public IActionResult Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var stats = new
            {
                Books = _context.Books.Count(),
                Authors = _context.Authors.Count(),
                Categories = _context.Categories.Count(),
                Orders = _context.Orders.Count()
            };
            ViewBag.Stats = stats;
            return View();
        }

        // Stored Procedure çağrısı: GetBookReport
        public IActionResult Report()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var reports = new List<BookReportViewModel>();
            var connStr = _configuration.GetConnectionString("DefaultConnection");

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("GetBookReport", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reports.Add(new BookReportViewModel
                            {
                                BookId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                ISBN = reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                AuthorName = reader.GetString(4),
                                CategoryName = reader.GetString(5),
                                Stock = reader.GetInt32(6)
                            });
                        }
                    }
                }
            }

            return View(reports);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
