using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Immutable;

public class UnitTest1 : TestBase {
    [Fact]
    public async Task ShouldUnableToSaveChange() {
        using var db = GetQueryContext();
        var user = new User {
            FirstName = "John",
            LastName = "Doe"
        };
        _ = await db.CreateAsync(user);
    }

    [Fact]
    public async Task PartialUpdate1() {
        using var db = GetQueryContext();
        var user = db.Users.First();
        var newUser = user with { FirstName = "UU 1" };

        Action<EntityEntry<User>> action = x => {
            x.Property(x => x.FirstName).IsModified = true;
        };

        _ = await db.UpdateAsync(newUser, action);
    }

    [Fact]
    public async Task PartialUpdate2() {
        using var queryDb = GetQueryContext();
        var currentUser = queryDb.Users.First();
        var newUser = currentUser with { FirstName = "UU 4", Age = DateTime.Now.Second };
        _ = await queryDb.UpdateAsync(currentUser, newUser);
    }


    [Fact]
    public async Task BulkUpdate() {
        using var db = GetQueryContext();
        var users = db.Users.Where(x => x.Id > 0);

        _ = users.ExecuteUpdate(user =>
            user.SetProperty(x => x.FirstName, x => "jw")
        );

        foreach (var item in users) {
            Assert.Equal("jw", item.FirstName);
        }
    }
}
