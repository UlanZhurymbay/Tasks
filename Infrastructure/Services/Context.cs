using System.Linq.Expressions;
using AppCore;
using AppCore.Exceptions;
using AppCore.Interfaces;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using MyTask = AppCore.Entities.Task;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Services;

public class Context : IContext
{
    private readonly TaskContext _dbContext;

    public Context(TaskContext dbContext)
    {
        _dbContext = dbContext;
    }
    public  IQueryable<T> GetAll<T>() where T : BaseEntity =>
        _dbContext.Set<T>();
    
    public async Task<T> FindAsync<T>(int id) where T : BaseEntity =>
        await _dbContext.Set<T>().FirstOrDefaultAsync(p => p.Id == id)
        ?? throw new NotFoundException();   
    public async Task<T> FindAsync<T>(int id, Expression<Func<T, List<MyTask>>> func) where T : BaseEntity =>
        await _dbContext.Set<T>().Include(func).FirstOrDefaultAsync(p => p.Id == id)
        ?? throw new NotFoundException();
    public async Task UpdateAsync<T>(T entity) where T : BaseEntity 
    {
        _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }    
    public async Task AddAsync<T>(T entity) where T : BaseEntity 
    {
        await _dbContext.Set<T>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }   
    public async Task RemoveAsync<T>(T entity) where T : BaseEntity 
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }  
}