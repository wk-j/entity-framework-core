using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbQuerySet {
    public class MyContext : DbContext {
        public DbSet<Faction> Factions { get; set; }
        public DbSet<Leader> Leaders { get; set; }

        public DbQuery<LeaderQuery> LeadersQuery { get; set; }
        public DbQuery<FactionQuery> FactionsQuery { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql(@"Server=localhost;Database=QueryTypesRepro;User Id=postgres;Password=1234;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
        }
    }

    public class Faction {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Leader> Leaders { get; set; }
    }

    public class Leader {
        public int Id { get; set; }
        public string Name { get; set; }
        public Faction Faction { get; set; }
    }

    [Table("Factions")]
    public class FactionQuery {
        public string Name { get; set; }
    }

    [Table("Leaders")]
    public class LeaderQuery {
        public string Name { get; set; }
    }

    class Program {
        static void Main(string[] args) {
            using (var context = new MyContext()) {
                context.Database.EnsureCreated();

                context.Factions.Add(new Faction { Name = "N1" });
                context.Factions.Add(new Faction { Name = "N2" });

                context.Leaders.Add(new Leader { Name = "N1" });
                context.Leaders.Add(new Leader { Name = "N2" });

                context.SaveChanges();

                var data = context.LeadersQuery.Where(x => x.Name == "N1");
                foreach (var item in data) {
                    Console.WriteLine($"Name = {item.Name}");
                }
            }
        }
    }
}
