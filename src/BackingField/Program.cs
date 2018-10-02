using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BackingField {

    public class Product {
        [Key]
        public int Id { set; get; }
        public string Name { private set; get; } = "Product";

        // private readonly int price;
        private int price;
    }

    class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) {

        }

        public DbSet<Product> Products { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Product>().Property("price");
        }

    }

    class Program {
        static void Main(string[] args) {
            var collection = new ServiceCollection();
            collection.AddDbContext<MyContext>(builder => {
                builder.UseNpgsql("Host=localhost;User Id=postgres;Password=1234;Database=DB");
            });

            var provider = collection.BuildServiceProvider();
            var context = provider.GetService<MyContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();


            var product = new Product();
            context.Products.Add(product);
            context.SaveChanges();

            var count = context.Products.Where(x => x.Name == "Product").Count();
            Console.WriteLine(count > 0);
        }
    }
}
