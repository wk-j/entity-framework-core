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
    }

    class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) {

        }

        public DbSet<Product> Products { set; get; }

    }

    class Program {
        static void Main(string[] args) {
            var collection = new ServiceCollection();
            collection.AddDbContext<MyContext>(builder => {
                builder.UseNpgsql("Host=localhost;User Id=postgres;Password=1234;Database=DB");
            });

            var provider = collection.BuildServiceProvider();
            var context = provider.GetService<MyContext>();
            context.Database.EnsureCreated();

            context.Database.EnsureCreated();


            var product = new Product();
            context.Products.Add(product);
            context.SaveChanges();

            var count = context.Products.Where(x => x.Name == "Product").Count();
            Console.WriteLine(count > 0);
        }
    }
}
