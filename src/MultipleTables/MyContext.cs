using Microsoft.EntityFrameworkCore;

namespace MultipleTables {
    class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) {
        }

        public DbSet<Person> Persons1 { set; get; }
        public DbSet<Person> Persons2 { set; get; }
    }
}
