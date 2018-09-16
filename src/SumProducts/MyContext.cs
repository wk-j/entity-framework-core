using Microsoft.EntityFrameworkCore;

namespace SumProducts {
    class MyContext : DbContext {
        public MyContext(DbContextOptions options) : base(options) {

        }

        public DbSet<Order> Orders { set; get; }
    }
}
