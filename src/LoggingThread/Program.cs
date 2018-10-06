using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace LoggingThread {

    public class Student {
        [Key]
        public int Id { set; get; }
        public string Name { set; get; }
        public float Gpa { set; get; }
    }

    class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<Student> Students { set; get; }
    }

    class MyProgram {
        private readonly MyContext context;

        public MyProgram(MyContext context) {
            this.context = context;
        }

        public void Run() {
            foreach (var item in Enumerable.Range(0, 10000)) {
                var student = context.Students.ToList();
                Console.WriteLine($"{item} {student.Count}");
            }
        }
    }

    class Program {
        static void Main(string[] args) {

            var collection = new ServiceCollection();
            collection.AddDbContext<MyContext>(builder => {
                builder.UseNpgsql("Host=localhost;User Id=postgres;Password=1234;Database=LoggingThread");
                var p = new ConsoleLoggerProvider((x, level) => level != LogLevel.Debug, true);
                var factory = new LoggerFactory(new[] { p });
                builder.UseLoggerFactory(factory);
            });

            collection.AddScoped<MyProgram>();

            var provider = collection.BuildServiceProvider();
            var program = provider.GetService<MyProgram>();

            var context = provider.GetService<MyContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            foreach (var item in Enumerable.Range(0, 100)) {
                context.Students.Add(new Student());
            }
            context.SaveChanges();

            program.Run();

            Console.ReadLine();
        }
    }
}
