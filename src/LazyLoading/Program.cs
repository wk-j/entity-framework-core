using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LazyLoading {
    class Query {
        private MyContext context;
        ILogger<Query> logger;
        public Query(MyContext context, ILogger<Query> logger) {
            this.context = context;
            this.logger = logger;
        }

        public void InitDb() {
            context.Database.EnsureCreated();
            if (!context.Students.Any()) {
                context.Students.Add(new Student { });
                context.Students.Add(new Student { });
                context.SaveChanges();
            }
        }
        public void QueryStudent() {
            context.Students.ToList();
        }
    }

    class Program {
        static void Main(string[] args) {
            var collection = new ServiceCollection();
            collection.AddDbContext<MyContext>(builder => {
                var c = "Host=localhost;User Id=postgres;Password=1234; Database=LazyLoading";
                builder.UseNpgsql(c);
            });
            collection.AddLogging(builder => {
                builder.AddConsole();
            });
            collection.AddScoped<Query>();

            var provider = collection.BuildServiceProvider();
            var context = provider.GetService<MyContext>();

            var query = provider.GetService<Query>();

            query.InitDb();
            query.QueryStudent();

        }
    }
}
