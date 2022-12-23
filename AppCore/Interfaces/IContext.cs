using System.Linq.Expressions;
using MyTask = AppCore.Entities.Task;

namespace AppCore.Interfaces;

public interface IContext
{
    public IQueryable<T> GetAll<T>() where T : BaseEntity;
    public Task<T> FindAsync<T>(int id) where T : BaseEntity;
    public Task<T> FindAsync<T>(int id, Expression<Func<T, List<MyTask>>> func) where T : BaseEntity;

    public Task UpdateAsync<T>(T entity) where T : BaseEntity;
    public Task AddAsync<T>(T entity) where T : BaseEntity;
    public Task RemoveAsync<T>(T entity) where T : BaseEntity;
}