using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace One2One {
    public class Person {
        public int Id { set; get; }
        public string Name { set; get; }
        public Address HomeAddress { get; set; }
        public Address WorkAddress { set; get; }
    }

    public class Address {
        public string Line1 { set; get; }
        public string Line2 { set; get; }
        public string City { set; get; }
        public string Region { set; get; }
        public string Country { set; get; }
        public string Postcode { set; get; }
    }

    public class PersonContext : DbContext {
        public PersonContext(DbContextOptions options) : base(options) {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Person>(b => {
                b.OwnsOne(x => x.HomeAddress, x => {
                    x.Property(x => x.Line1).IsRequired();
                    x.Property(x => x.City).IsRequired();
                });
                b.Navigation(x => x.HomeAddress).IsRequired();
                b.OwnsOne(x => x.WorkAddress);
            });
        }

        public DbSet<Person> Persons { set; get; }
    }

    public class Program {
        static void Main(string[] args) {
            var collection = new ServiceCollection();
            collection.AddDbContext<PersonContext>(opt => {
                opt.UseNpgsql("Host=localhost;User Id=postgres;Password=1234;Database=One2One", x => {
                    x.SetPostgresVersion(9, 4);
                });
            });

            var context = collection.BuildServiceProvider().GetService<PersonContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Persons.Add(new Person {
                HomeAddress = new Address {
                    Line1 = "lin1",
                    City = "city"
                },
                WorkAddress = new Address { }
            });

            context.SaveChanges();


        }
    }
}
