using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MultipleTables {

    class Program {
        static void Main(string[] args) {

            var colection = new ServiceCollection();
            colection.AddDbContext<MyContext>(b => {
                var conn = "Host=localhost; Database=MultipeTables;Username=postgres;Password=1234";
                b.UseNpgsql(conn);
            });

            var provider = colection.BuildServiceProvider();
            var context = provider.GetService<MyContext>();
            context.Database.EnsureCreated();
            context.Persons1.Add(new Person { Name = "Person1" });
            context.Persons2.Add(new Person { Name = "Person2" });
            context.SaveChanges();
        }
    }
}
