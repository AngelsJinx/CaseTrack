using CaseTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CaseTrack.Data.Repositories;

/// <summary>
/// Parent class for all repositories to inherit from.
/// </summary>
/// <param name="context">Database context.</param>
/// <typeparam name="T">The entity type that can be managed by this repository.</typeparam>
public abstract class BaseRepository<T>(CaseTrackContext context) : IRepository<T> where T : BaseEntity
{
    protected abstract DbSet<T> GetDbSet();

    public IQueryable<T> GetQueryable()
    {
        return GetDbSet();
    }

    public Task<T?> Get(long id)
    {
        return GetQueryable().SingleOrDefaultAsync(x => x.Id == id);
    }

    public ValueTask<EntityEntry<T>> Add(T task)
    {
        return GetDbSet().AddAsync(task);
    }

    public EntityEntry<T> Remove(T task)
    {
        // This hard-deletes the task row from the database.
        // However it could instead soft-delete by setting a deletedAt timestamp or equivalent.
        // If soft-deletes were implemented, the GetQueryable() method should be updated to take an optional 'includeDeleted' parameter, and exclude deleted rows by default.
        // This could be applied to all entities, or handled by a SoftDeleteRepository child class.
        return GetDbSet().Remove(task);
    }

    public Task SaveChanges()
    {
        return context.SaveChangesAsync();
    }
}