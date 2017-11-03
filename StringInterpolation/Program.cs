using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace StringInterpolation
{
    public class MyLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger();
        }

        public void Dispose()
        { }

        private class MyLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                Console.WriteLine(formatter(state, exception));
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }

    public class QPerson
    {
        [Key]
        public int Id { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
    }

    public class MyContext : DbContext
    {
        public MyContext() { }
        public DbSet<QPerson> Persons { set; get; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host = localhost; Database=ef;Username=wk;Password=");
            //optionsBuilder.UseMySQL("server=localhost;userid=root;pwd=1234;port=3306;database=ef");
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\v11.0;Database=NORTHWIND;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new MyContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var serviceProvider = context.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(new MyLoggerProvider());

                context.Persons.Add(new QPerson
                {
                    FirstName = "wk",
                    LastName = "kw"
                });

                context.SaveChanges();

                var name = "wk";
                //var rs = context.Persons.FromSql($"SELECT * FROM Persons WHERE FirstName = {name}");
                var rs = context.Persons.ToListAsync(default(CancellationToken)).Result;
                foreach (var rr in rs)
                {
                    Console.WriteLine($"{rr.Id} {rr.FirstName} {rr.LastName}");
                }
            }
        }
    }
}
