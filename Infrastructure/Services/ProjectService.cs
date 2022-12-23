using AppCore.Entities;
using AppCore.Exceptions;
using AppCore.Interfaces;
using AppCore.ViewModel;
using Infrastructure.DataContext;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Services;

public class ProjectService : IProject
{
    private readonly TaskContext _dbContext;
    private readonly ContextHelper _contextHelper;
    private readonly StateHelper _stateHelper;

    public ProjectService(TaskContext dbContext, ContextHelper contextHelper, StateHelper stateHelper)
    {
        _dbContext = dbContext;
        _contextHelper = contextHelper;
        _stateHelper = stateHelper;
    }

    public async Task<IActionResult> GetAllAsync()
    {
        return new OkObjectResult(await _contextHelper.Projects().ToListAsync());
    }


    public async Task<IActionResult> FindAsync(int id)
    {
        return new OkObjectResult(await _contextHelper.ProjectWithTasksAsync(id));
    }

    public async Task<IActionResult> AddAsync(ProjectViewModel model)
    {
        var project = new Project(model.Name, model.Description, model.Priority);
        await _dbContext.Projects.AddAsync(project);
        await _dbContext.SaveChangesAsync();
        return new OkObjectResult(project);
    }

    public async Task<IActionResult> EditAsync(ProjectEditViewModel model, int id)
    {
        var project = await _contextHelper.ProjectAsync(id);
        if (project is null)
        {
            return new NotFoundObjectResult(ContextHelper.NotFoundProject);
        }
        await _contextHelper.UpdateProjectAsync(project.Edit(model.Name, model.Description));
        return new OkObjectResult(project);
    }

    public async Task<IActionResult> ChangeStateAsync(ProjectStateViewModel model, int id)
    {
        var project = await _contextHelper.ProjectAsync(id);
        if (project is null)
        {
            return new NotFoundObjectResult(ContextHelper.NotFoundProject);
        }

        var errorMessage = _stateHelper.ChangeStateMessage(project.State, model.State);
        if (!string.IsNullOrEmpty(errorMessage))
        {
            return new BadRequestObjectResult(errorMessage);
        }
        await _contextHelper.UpdateProjectAsync(project.ChangeState(model.State));
        return new OkResult();
    }

    public async Task<IActionResult> RemoveAsync(int id)
    {
        _dbContext.Projects.Remove(await _contextHelper.ProjectAsync(id));
        await _dbContext.SaveChangesAsync();
        return new OkResult();
    }

    public async Task<IActionResult> RemoveTaskAsync(int projectId, int taskId)
    {
        var project = await _contextHelper.ProjectWithTasksAsync(projectId);
        var task = await _contextHelper.TaskAsync(taskId);
        if (!project.Tasks.Any(t => t == task))
        {
            return new NotFoundObjectResult(ContextHelper.NotHasTask);
        }
        await _contextHelper.UpdateProjectAsync(project.RemoveTask(task));
        return new OkResult();
    }
}