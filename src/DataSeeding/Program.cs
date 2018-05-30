using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework21 {
    class Person {
        [Key]
        public int Id { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
    }

    internal class MyContext : DbContext {
        internal MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Person> Persons { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, FirstName = "wk1", LastName = "wk1" },
                new Person { Id = 2, FirstName = "wk2", LastName = "wk2" },
                new Person { Id = 3, FirstName = "wk3", LastName = "wk3" }
            );
        }
    }

    class Program {
        static void Main(string[] args) {
            var connectionString = "Host=localhost;Username=postgres;Password=1234;Database=DataSeeding";
            var options = new DbContextOptionsBuilder<MyContext>();
            options.UseNpgsql(connectionString);

            var context = new MyContext(options.Options);
            context.Database.EnsureCreated();
        }
    }
}
