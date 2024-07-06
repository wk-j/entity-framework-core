using Microsoft.EntityFrameworkCore;

namespace Immutable;

public class QueryTest : TestBase {
    [Fact]
    public void TestCreate() {
        var context = GetQueryContext();
        context.Database.EnsureCreated();
    }

    [Fact]
    public void TestDelete() {
        var context = GetQueryContext();
        context.Database.EnsureDeleted();
    }

    [Fact]
    public void TestInsert() {
        var file = new File {
            Name = "wk",
            Properties = [
                new Property {
                    Name = "a1",
                    IntValue = 123,
                    Type = PropertyType.Int
                },
                new Property {
                    Name = "a2",
                    StringValue = "123",
                    Type = PropertyType.String
                }
            ]
        };


        var db = GetQueryContext();

        db.Attach(file);
        db.Files.Add(file);
        db.SaveChanges();

    }

    [Fact]
    public void TestQuery() {
        var db = GetQueryContext();
        var name = "a1";
        var intValue = 123;
        string? stringValue = null;
        DateTime? dateValue = null;

        var files = db.Files
            .Include(file => file.Properties)
            .Where(x => x.Properties.Any(p =>
                p.Name == name &&
                (
                    (p.Type == PropertyType.Int && p.IntValue == intValue) ||
                    (p.Type == PropertyType.String && p.StringValue == stringValue) ||
                    (p.Type == PropertyType.DateTime && p.DateTimeValue == dateValue)
                )
            )
        );

        foreach (var file in files) {
            Console.WriteLine(file.Name);
        }
    }
}
