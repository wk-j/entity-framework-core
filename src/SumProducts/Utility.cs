using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SumProducts {
    public class Utility {
        public static bool Init(string connectionString) {
            var collection = new ServiceCollection();
            collection.AddDbContext<MyContext>(options => {
                options.UseMySql(connectionString);
            });
            var provider = collection.BuildServiceProvider();
            var context = provider.GetService<MyContext>();

            CreateTable(context);
            if (!context.Orders.Any()) {
                Insert(context, 1_000_000);
                return false;
            }
            return true;
        }

        static void Insert(MyContext context, int count) {
            var orders = Enumerable.Range(0, count).Select(item => {
                var random = new Random();
                var date = new DateTime(random.Next(2000, 2019), random.Next(1, 13), random.Next(1, 28));
                var order = new Order {
                    LastUpdate = date,
                    Total = random.Next(100)
                };
                return order;
            });
            context.Orders.AddRange(orders);
            context.SaveChanges();
        }

        static void CreateTable(MyContext context) {
            context.Database.EnsureCreated();
        }
    }
}