using AppCore.Entities;
using AppCore.Exceptions;
using AppCore.Interfaces;
using AppCore.Messages;
using AppCore.ViewModel;
using Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task = AppCore.Entities.Task;

namespace Tasks.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController : Controller
{
    private readonly IContext _context;

    public ProjectController(IContext project)
    {
        _context = project;
    }

    [HttpGet("Projects")]
    public async Task<IActionResult> GetAllAsync()
    {
        var projects = await _context.GetAll<Project>()
            .Include(p => p.Tasks)
            .ToListAsync();
        return new OkObjectResult(projects);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var project = await _context.FindAsync<Project>(id, p => p.Tasks);
            return new OkObjectResult(project);
        }
        catch (NotFoundException)
        {
            return new NotFoundObjectResult(Message.NotFoundProject);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(ProjectViewModel model)
    {
        var project = new Project(model.Name, model.Description, model.Priority);
        await _context.AddAsync(project);
        return new OkObjectResult(project);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditAsync(ProjectEditViewModel model, int id)
    {
        try
        {
            var project = await _context.FindAsync<Project>(id);
            await _context.UpdateAsync(project.Edit(model.Name, model.Description));
            return new OkObjectResult(project);
        }
        catch (NotFoundException)
        {
            return new NotFoundObjectResult(Message.NotFoundProject);
        }
    }

    [HttpPut("ChangeState/{id:int}")]
    public async Task<IActionResult> ChangeStateAsync(ProjectStateViewModel model, int id)
    {
        try
        {
            var project = await _context.FindAsync<Project>(id);
            var errorMessage = StateHelper.ChangeStateMessage(project.State, model.State);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new BadRequestObjectResult(errorMessage);
            }
            await _context.UpdateAsync(project.ChangeState(model.State));
            return new OkObjectResult(project);
        }
        catch (NotFoundException)
        {
            return new NotFoundObjectResult(Message.NotFoundProject);
        }
    }


    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            await _context.RemoveAsync(await _context.FindAsync<Project>(id));
            return new OkResult();
        }
        catch (NotFoundException)
        {
            return new NotFoundObjectResult(Message.NotFoundProject);
        }
    }

    [HttpDelete("DeleteTask/{projectId:int}")]
    public async Task<IActionResult> DeleteTaskAsync(int projectId, int taskId)
    {
        try
        {
            var project = await _context.FindAsync<Project>(projectId, p => p.Tasks);
            var task = await _context.FindAsync<Task>(taskId);
            if (!project.Tasks.Any(t => t == task))
            {
                return new NotFoundObjectResult(Message.NotHasTask);
            }
            await _context.UpdateAsync(project.RemoveTask(task));
            return new OkResult();
        }
        catch (NotFoundException)
        {
            return new NotFoundObjectResult(Message.NotFoundProjectOrTask);
        }
    }
}