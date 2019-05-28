using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AutoIncrease {
    public class Student {
        [Key]
        public int Id { set; get; }
        public string Name { set; get; }
    }

    class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Student> Students { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder
                .HasSequence<int>("StudentId")
                .StartsAt(1000);

            modelBuilder.Entity<Student>().Property(e => e.Id)
                .HasDefaultValueSql(@"nextval('""StudentId""')");
        }
    }

    class Program {
        static void Main(string[] args) {
            var builder = new DbContextOptionsBuilder();
            builder.UseNpgsql("Host=localhost;User Id=postgres;Password=1234;Database=XX");
            var options = builder.Options;

            using (var context = new MyContext(options)) {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Students.Add(new Student {
                    Id = 1,
                    Name = "Hello"
                });
                context.SaveChanges();
            }

            using (var context = new MyContext(options)) {
                context.Students.Add(new Student { });
                context.Students.Add(new Student { });
                context.Students.Add(new Student { });
                context.SaveChanges();
            }
        }
    }
}