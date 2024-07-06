using Microsoft.EntityFrameworkCore;

namespace Immutable;

public class MyContext(DbContextOptions<MyContext> context) : DbContext(context) {
    public DbSet<File> Files { set; get; }
    public DbSet<Property> Properties { set; get; }
}
