using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Immutable;

public record Entity {
    [Key]
    public int Id { get; init; }
}

public record User : Entity {
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}

public class MyContext(DbContextOptions<MyContext> context) : DbContext(context) {
    public DbSet<User> Users { set; get; }
}

public class ContextService {
    public MyContext Create() {
        var builder = new DbContextOptionsBuilder<MyContext>();
        builder
            .UseNpgsql("Host=localhost;Password=1234;User Id=postgres;Database=Im")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        return new MyContext(builder.Options);
    }
}

public class TestBase {
    protected MyContext GetContext() {
        var contextService = new ContextService();
        var db = contextService.Create();
        _ = db.Database.EnsureCreated();
        return db;
    }
}

public class UnitTest1 : TestBase {
    [Fact]
    public async Task ShouldUnableToSaveChange() {
        var db = GetContext();

        var user = new User {
            FirstName = "John",
            LastName = "Doe"
        };

        await db.SaveEntityStateAsync(user, EntityState.Added);
    }

    [Fact]
    public async Task Query() {
        using var db = GetContext();
        var users = db.Users.ToList();

        foreach (var user in users) {
            var newUser = user with { FirstName = "wk" };
            await db.SaveEntityStateAsync(newUser, EntityState.Modified);
        }
    }
}
