using Microsoft.EntityFrameworkCore;
using LibraryMS.Models;

namespace LibraryMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Username = "admin", Password = "Admin123!", Email = "admin@libraryms.com", Role = "Admin" },
                new User { UserId = 2, Username = "user", Password = "User123!", Email = "user@libraryms.com", Role = "User" }
            );

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Roman", Description = "Kurgu ve edebiyat romanları" },
                new Category { CategoryId = 2, Name = "Bilim & Teknoloji", Description = "Bilim ve teknoloji kitapları" },
                new Category { CategoryId = 3, Name = "Tarih", Description = "Tarih ve biyografi kitapları" },
                new Category { CategoryId = 4, Name = "Kişisel Gelişim", Description = "Kişisel gelişim kitapları" }
            );

            // Seed Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { AuthorId = 1, FirstName = "Orhan", LastName = "Pamuk", Bio = "Nobel ödüllü Türk romancı.", Email = "pamuk@example.com", BirthDate = new DateTime(1952, 6, 7) },
                new Author { AuthorId = 2, FirstName = "Yaşar", LastName = "Kemal", Bio = "Türk edebiyatının dev ismi.", Email = "kemal@example.com", BirthDate = new DateTime(1923, 10, 6) },
                new Author { AuthorId = 3, FirstName = "Sabahattin", LastName = "Ali", Bio = "Türk hikâye ve roman yazarı.", Email = "ali@example.com", BirthDate = new DateTime(1907, 2, 25) }
            );
        }
    }
}
