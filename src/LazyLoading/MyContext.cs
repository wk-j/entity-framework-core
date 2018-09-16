using Microsoft.EntityFrameworkCore;

class MyContext : DbContext {
    public MyContext(DbContextOptions options) : base(options) {
    }

    public DbSet<Student> Students { set; get; }
    public DbSet<Enrollment> Enrollments { set; get; }
    public DbSet<Course> Courses { set; get; }
    public DbSet<SportsActivity> SportActivities { set; get; }
    public DbSet<Sports> Sports { set; get; }
}