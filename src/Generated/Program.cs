using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Generated {

    public class Student {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { set; get; }

        public DateTime Inserted { set; get; }

        public DateTime Updated { set; get; }
    }

    public class Context : DbContext {
        public Context(DbContextOptions options) : base(options) {

        }

        public DbSet<Student> Students { set; get; }

        protected override void OnModelCreating(ModelBuilder builder) {
            // builder.Entity<Student>()
            //     .Property(x => x.Inserted)
            //     .HasColumnType("timestamp without time zone")
            //     .HasDefaultValue("NOW()");
            // .ValueGeneratedOnAddOrUpdate();


            // builder.Entity<Student>()
            //     .Property(x => x.Updated)
            //     .HasColumnType("timestamp without time zone")
            //     .HasDefaultValue("NOW()");
            // .ValueGeneratedOnAddOrUpdate();
        }
    }

    class Program {
        static void Main(string[] args) {
            var connection = "Host=localhost;User Id=postgres;Password=1234;Database=generated";

            var fact = LoggerFactory.Create(b => {
                b.AddConsole();
            });

            var options = new DbContextOptionsBuilder()
                .UseLoggerFactory(fact)
                .UseNpgsql(connection).Options;

            using (var context = new Context(options)) {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Students.Add(new Student { });
                context.SaveChanges();
            }
        }
    }
}
