using System;
using MySql.Data.MySqlClient;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections.Generic;
using DynamicTables;

namespace SumProducts {

    class Result2 {
        public int Total { set; get; }
        public string Month { set; get; }
        public int Year { set; get; }
    }

    class Program {

        static List<Result2> Query2(string connectionString) {
            var connection = new MySqlConnection(connectionString);
            connection.Open();

            var sql = @"
SELECT SUM(Total) as Total, MONTHNAME(LastUpdate) as Month, YEAR(LastUpdate) as Year
FROM Orders

GROUP BY Month, Year";
            var rs = connection.Query<Result2>(sql).ToList();
            connection.Close();
            return rs;
        }


        static List<Result> Query1(string connectionString) {
            var connection = new MySqlConnection(connectionString);
            connection.Open();

            var sql = @"
SELECT
YEAR(LastUpdate) AS Year,
SUM(IF(MONTH(LastUpdate)=1,Total,NULL)) AS A1,
SUM(IF(MONTH(LastUpdate)=2,Total,NULL)) AS A2,
SUM(IF(MONTH(LastUpdate)=3,Total,NULL)) AS A3,
SUM(IF(MONTH(LastUpdate)=4,Total,NULL)) AS A4,
SUM(IF(MONTH(LastUpdate)=5,Total,NULL)) AS A5,
SUM(IF(MONTH(LastUpdate)=6,Total,NULL)) AS A6,
SUM(IF(MONTH(LastUpdate)=7,Total,NULL)) AS A7,
SUM(IF(MONTH(LastUpdate)=8,Total,NULL)) AS A8,
SUM(IF(MONTH(LastUpdate)=9,Total,NULL)) AS A9,
SUM(IF(MONTH(LastUpdate)=10,Total,NULL)) AS A10,
SUM(IF(MONTH(LastUpdate)=11,Total,NULL)) AS A11,
SUM(IF(MONTH(LastUpdate)=12,Total,NULL)) AS A12
FROM Orders
GROUP BY Year";

            var rs = connection.Query<Result>(sql).ToList();
            connection.Close();
            return rs;
        }

        static void Insert(MyContext context) {
            var range = Enumerable.Range(0, 100_000);
            var random = new Random();
            foreach (var item in range) {
                var date = new DateTime(random.Next(2000, 2019), random.Next(1, 13), random.Next(1, 28));
                var order = new Order {
                    LastUpdate = date,
                    Total = random.Next(100)
                };
                context.Add(order);
            }
            context.SaveChanges();
        }

        static void CreateTable(MyContext context) {
            context.Database.EnsureCreated();

            if (!context.Orders.Any()) {
                Insert(context);
            }
        }

        static void Main(string[] args) {
            var connectionString = "Host=localhost;User=root;Password=1234;Database=A1234";
            var collection = new ServiceCollection();

            collection.AddDbContext<MyContext>(options => {
                options.UseMySql(connectionString);
            });

            var provider = collection.BuildServiceProvider();
            var context = provider.GetService<MyContext>();
            CreateTable(context);

            var rs = Query2(connectionString);
            DynamicTable.From(rs).Write();
        }
    }
}
