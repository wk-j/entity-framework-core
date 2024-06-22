using Microsoft.EntityFrameworkCore;

namespace Immutable;

public static class DbContextExtensions {
    public static async Task<TEntity> SaveEntityStateAsync<TEntity>(
        this DbContext context,
        TEntity entity,
        EntityState state,
        CancellationToken ct = default
    ) where TEntity : Entity {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var entry = context.Entry(entity);
        entry.State = state;

        await context.SaveChangesAsync(ct);

        entry.State = EntityState.Detached;

        return entry.Entity;
    }
}
