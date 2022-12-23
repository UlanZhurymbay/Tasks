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
public class TaskController : Controller
{

    private readonly IContext _context;


    public TaskController(IContext context)
    {
        _context = context;
    }

    [HttpGet("Tasks")]
    public async Task<IActionResult> GetAllAsync()
    {
        var tasks = await _context.GetAll<Task>().ToListAsync();
         return new OkObjectResult(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        try
        {
            var task = await _context.FindAsync<Task>(id);
            return new OkObjectResult(task);
        }
        catch (NotFoundException)
        {
            return new NotFoundObjectResult(Message.NotFoundTask);
        }
    }
        
    [HttpPost]
    public async Task<IActionResult> AddAsync(TaskViewModel model)
    {
        try
        {       
            var project = await _context.FindAsync<Project>(model.ProjectId);
            var task = new Task(model.Name, model.Description, model.Priority);
            await _context.UpdateAsync(project.AddTask(task));
            return new ObjectResult(project);
        }
        catch (NotFoundException)
        {
            return new NotFoundObjectResult(Message.NotFoundProject);
        }
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditAsync(TaskEditViewModel model, int id)
    {
        try
        {
            var task = await _context.FindAsync<Task>(id);
            await _context.UpdateAsync(task.Edit(model.Name, model.Description));
            return new OkObjectResult(task);
        }
        catch (NotFoundException)
        {
            return new NotFoundObjectResult(Message.NotFoundTask);
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var task = await _context.FindAsync<Task>(id);
            await _context.RemoveAsync(task);
            return new OkResult();
        }
        catch (NotFoundException)
        {
            return new NotFoundObjectResult(Message.NotFoundTask);
        }
    }
    
    [HttpPut("ChangeState/{id:int}")]
    public async Task<IActionResult> ChangeStateAsync(TaskStateViewModel model, int id)
    {
        try
        {
            var task = await _context.FindAsync<Task>(id);
            var errorMessage = StateHelper.ChangeStateMessage(task.State, model.State);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new BadRequestObjectResult(errorMessage);
            }
            await _context.UpdateAsync(task.ChangeState(model.State));
            return new OkObjectResult(task);
        }
        catch (NotFoundException)
        {
            return new NotFoundObjectResult(Message.NotFoundTask);
        }
    }
}