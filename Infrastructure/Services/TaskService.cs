using AppCore.Interfaces;
using AppCore.ViewModel;
using Infrastructure.DataContext;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = AppCore.Entities.Task;

namespace Infrastructure.Services;

public class TaskService : ITask
{
    private readonly TaskContext _dbContext;
    private readonly ContextHelper _contextHelper;
    private readonly StateHelper _stateHelper;

    public TaskService(TaskContext dbContext, ContextHelper contextHelper, StateHelper stateHelper)
    {
        _dbContext = dbContext;
        _contextHelper = contextHelper;
        _stateHelper = stateHelper;
    }

    public async Task<IActionResult> GetAllAsync()
    {
        return new OkObjectResult(await _dbContext.Tasks.ToListAsync());
    }

    public async Task<IActionResult> FindAsync(int id)
    {
        return new OkObjectResult(await _contextHelper.TaskAsync(id));
    }

    public async Task<IActionResult> AddAsync(TaskViewModel model)
    {
        var project = await _contextHelper.ProjectAsync(model.ProjectId);
        await _contextHelper
            .UpdateProjectAsync(project.AddTask(new Task(model.Name, model.Description, model.Priority)));
        return new ObjectResult(project);
    }
    
    public async Task<IActionResult> EditAsync(TaskEditViewModel model, int id)
    {
        var task = await _contextHelper.TaskAsync(id);
        await _contextHelper.UpdateTaskAsync(task.Edit(model.Name, model.Description));
        return new OkObjectResult(task);
    }
    
    public async Task<IActionResult> RemoveAsync(int id)
    {
        _dbContext.Tasks.Remove(await _contextHelper.TaskAsync(id));
        await _dbContext.SaveChangesAsync();
        return new OkResult();
    }

    
    public async Task<IActionResult> ChangeStateAsync(TaskStateViewModel model, int id)
    {
        var task = await _contextHelper.TaskAsync(id);
        var errorMessage = _stateHelper.ChangeStateMessage(task.State, model.State);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            return new BadRequestObjectResult(errorMessage);
        }
        await _contextHelper.UpdateTaskAsync(task.ChangeState(model.State));
        return new OkResult();
    }
    

}