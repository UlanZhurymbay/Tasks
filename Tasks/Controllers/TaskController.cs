using AppCore.Exceptions;
using AppCore.Interfaces;
using AppCore.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Tasks.Controllers;
[ApiController]
[Route("[controller]")]
public class TaskController : Controller
{

    private readonly ITask _task;


    public TaskController(ITask task)
    {
        _task = task;
    }

    [HttpGet("Tasks")]
    public async Task<IActionResult> GetAllAsync()
    {
         return await _task.GetAllAsync();

    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTaskAsync(int id)
    {
        try
        {
            return await _task.FindAsync(id);
        }
        catch (NotFoundException e)
        {
            return new NotFoundObjectResult(e.Message);
        }
    }
        
    [HttpPost]
    public async Task<IActionResult> AddAsync(TaskViewModel model)
    {
        try
        {
            return await _task.AddAsync(model);
        }
        catch (NotFoundException e)
        {
            return new NotFoundObjectResult(e.Message);
        }
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditAsync(TaskEditViewModel model, int id)
    {
        try
        {
            return await _task.EditAsync(model, id);
        }
        catch (NotFoundException e)
        {
            return new NotFoundObjectResult(e.Message);
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            return await _task.RemoveAsync(id);
        }
        catch (NotFoundException e)
        {
            return new NotFoundObjectResult(e.Message);
        }
    }
    
    [HttpPut("ChangeState/{id:int}")]
    public async Task<IActionResult> ChangeStateAsync(TaskStateViewModel model, int id)
    {
        try
        {
            return await _task.ChangeStateAsync(model, id);
        }
        catch (NotFoundException e)
        {
            return new NotFoundObjectResult(e.Message);
        }
    }
}