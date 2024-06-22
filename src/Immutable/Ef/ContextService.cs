using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Immutable;

public class ContextService {
    public MyContext Create() {
        var builder = new DbContextOptionsBuilder<MyContext>();
        builder
            .UseNpgsql("Host=localhost;Password=1234;User Id=postgres;Database=Im")
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        return new MyContext(builder.Options);
    }
}
