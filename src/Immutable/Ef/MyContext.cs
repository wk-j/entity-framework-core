using Microsoft.EntityFrameworkCore;

namespace Immutable;

public class MyContext(DbContextOptions<MyContext> context) : DbContext(context) {
    public DbSet<User> Users { set; get; }
}
