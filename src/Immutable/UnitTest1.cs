using Microsoft.EntityFrameworkCore;

namespace Immutable;

public class UnitTest1 : TestBase {
    [Fact]
    public async Task ShouldUnableToSaveChange() {
        using var db = GetContext();
        var user = new User {
            FirstName = "John",
            LastName = "Doe"
        };
        _ = await db.CreateAsync(user);
    }

    [Fact]
    public async Task ForceUpdate() {
        using var db = GetContext();
        var users = db.Users.ToList();

        foreach (var user in users) {
            var newUser = user with { FirstName = "wk" };
            // _ = await db.UpdateAsync(newUser);
        }
    }

    [Fact]
    public async Task PartialUpdate() {
        using var db = GetContext();
        var user = db.Users.First();
        var newUser = user with { FirstName = "UU" };

        _ = await db.UpdateAsync(newUser,
            x => x.Property(x => x.FirstName).IsModified = true
        );
    }

    [Fact]
    public async Task BulkUpdate() {
        using var db = GetContext();
        var users = db.Users.Where(x => x.Id > 0);

        _ = users.ExecuteUpdate(user =>
            user.SetProperty(x => x.FirstName, x => "jw")
        );

        foreach (var item in users) {
            Assert.Equal("jw", item.FirstName);
        }
    }
}
