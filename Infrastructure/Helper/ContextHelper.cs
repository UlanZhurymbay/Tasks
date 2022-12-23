using AppCore.Entities;
using AppCore.Exceptions;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyTask = AppCore.Entities.Task;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Helper;

public class ContextHelper
{
    private readonly TaskContext _dbContext;
    public const string NotFoundProject = "Not Found Project";
    public const string NotFoundTask = "Not Found Task";
    public const string NotHasTask = "Does not have such a Task";

    public ContextHelper(TaskContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Project> ProjectAsync(int id) =>
        await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id) 
        ?? throw new NotFoundException(NotFoundProject);

    public async Task<Project> ProjectWithTasksAsync(int id) =>
        await Projects().FirstOrDefaultAsync(p => p.Id == id)
        ?? throw new NotFoundException(NotFoundProject);  
    
    public IIncludableQueryable<Project, List<MyTask>> Projects() =>
         _dbContext.Projects.Include(p => p.Tasks);

    public async Task<MyTask> TaskAsync(int id) =>
        await _dbContext.Tasks.FirstOrDefaultAsync(p => p.Id == id) 
        ?? throw new NotFoundException(NotFoundTask);
    
    public async Task UpdateProjectAsync(Project project)
    {
        _dbContext.Projects.Update(project);
        await _dbContext.SaveChangesAsync();
    }    
    public async Task UpdateTaskAsync(MyTask task)
    {
        _dbContext.Tasks.Update(task);
        await _dbContext.SaveChangesAsync();
    }
}