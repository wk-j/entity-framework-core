using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Immutable;

public class ContextService {
    public MyContext CreateDbContext(QueryTrackingBehavior queryTrackingBehavior) {
        var builder = new DbContextOptionsBuilder<MyContext>();
        builder
            .UseNpgsql("Host=localhost;Password=1234;User Id=postgres;Database=Im")
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .UseQueryTrackingBehavior(queryTrackingBehavior);
        return new MyContext(builder.Options);
    }

    public MyContext CreateQueryContext() {
        return CreateDbContext(QueryTrackingBehavior.NoTracking);
    }

    public MyContext CreateUpdateContext() {
        return CreateDbContext(QueryTrackingBehavior.TrackAll);
    }
}
