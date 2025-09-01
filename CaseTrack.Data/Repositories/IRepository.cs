using CaseTrack.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CaseTrack.Data.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    IQueryable<T> GetQueryable();
    Task<T?> Get(long id);
    ValueTask<EntityEntry<T>> Add(T entity);
    EntityEntry<T> Remove(T entity);
    Task SaveChanges();
}