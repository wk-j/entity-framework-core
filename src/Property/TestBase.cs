namespace Immutable;

public class TestBase {
    protected MyContext GetQueryContext() {
        var contextService = new ContextService();
        var db = contextService.CreateQueryContext();
        return db;
    }

    // protected MyContext GetUpdateContext() {
    //     var contextService = new ContextService();
    //     var db = contextService.CreateUpdateContext();
    //     return db;
    // }
}
