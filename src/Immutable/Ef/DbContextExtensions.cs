using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Immutable;

public static class DbContextExtensions {

    public static async Task<TEntity> UpdateAsync<TEntity>(this DbContext context,
                                                           TEntity entity,
                                                           Action<EntityEntry<TEntity>> action) where TEntity : Entity {
        var entry = context.Entry<TEntity>(entity);
        action(entry);
        _ = await context.SaveChangesAsync();
        entry.State = EntityState.Detached;
        return entry.Entity;
    }

    public static async Task<TEntity> UpdateAsync<TEntity>(
        this DbContext context,
        TEntity currentEnty,
        TEntity newEntity,
        CancellationToken ct = default) where TEntity : Entity {

        try {
            context.Attach(currentEnty);
            context.Entry(currentEnty).CurrentValues.SetValues(newEntity);
            _ = context.SaveChanges();

            return await Task.FromResult(newEntity);
        } catch (Exception ex) {
            throw;
        }
    }

    public static async Task<TEntity> CreateAsync<TEntity>(
            this DbContext context,
            TEntity entity,
            CancellationToken ct = default
        ) where TEntity : Entity {
        return await context.SaveEntityStateAsync(
            entity,
            EntityState.Added,
            ct
        );
    }

    // public static async Task<TEntity> UpdateAsync<TEntity>(
    //     this DbContext context,
    //     TEntity entity,
    //     CancellationToken ct = default
    // ) where TEntity : Entity => await context.SaveEntityStateAsync(
    //     entity,
    //     EntityState.Modified,
    //     ct
    // );

    public static async Task DeleteAsync<TEntity>(
        this DbContext context,
        TEntity entity,
        CancellationToken ct = default
    ) where TEntity : Entity {
        _ = await context.SaveEntityStateAsync(entity, EntityState.Deleted, ct);
    }

    private static async Task<TEntity> SaveEntityStateAsync<TEntity>(
        this DbContext context,
        TEntity entity,
        EntityState state,
        CancellationToken ct = default
    ) where TEntity : Entity {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(entity);

        var entry = context.Entry(entity);
        entry.State = state;

        _ = await context.SaveChangesAsync(ct);

        entry.State = EntityState.Detached;

        return entry.Entity;
    }
}
