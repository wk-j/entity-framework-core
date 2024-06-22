namespace Immutable;

public class TestBase {
    protected MyContext GetContext() {
        var contextService = new ContextService();
        var db = contextService.Create();
        _ = db.Database.EnsureCreated();
        return db;
    }
}
