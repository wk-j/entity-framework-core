using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Array {
    public class Library {
        [Key]
        public int Id { set; get; }
        public List<string> Books { set; get; }
    }

    public class LibraryContext : DbContext {
        public DbSet<Library> Libraries { set; get; }

        public LibraryContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            var splitStringConverter = new ValueConverter<List<string>, string>(
                   v => string.Join(";", v),
                   v => v.Split(new[] { ';' }).ToList()
               );

            builder.Entity<Library>()
                .Property(nameof(Library.Books))
                .HasConversion(splitStringConverter);
        }
    }



    class Program {

        static void Insert(DbContextOptions options) {
            using var context = new LibraryContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var library = new Library {
                Books = new List<string> {
                    "Book 1",
                    "Book 2",
                    "Book 3"
                }
            };

            context.Libraries.Add(library);
            context.SaveChanges();
        }

        static void Query(DbContextOptions options) {
            using var context = new LibraryContext(options);

            var data = context.Libraries.Where(x => x.Books.Contains("Book 1"));
            foreach (var item in data) {
                Console.WriteLine(string.Join(";", item.Books));
            }
        }

        static void Main(string[] args) {
            var service = new ServiceCollection();
            service.AddLogging(builder => builder.AddConsole());

            var provider = service.BuildServiceProvider();
            var factory = provider.GetService<ILoggerFactory>();

            var options = new DbContextOptionsBuilder()
                .UseNpgsql("Host=localhost;User Id=postgres;Password=1234;Database=Library")
                .EnableSensitiveDataLogging(true)
                .UseLoggerFactory(factory)
                .Options;

            Insert(options);
            Query(options);
        }
    }
}