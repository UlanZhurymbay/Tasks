using AppCore.Entities;
using AppCore.Exceptions;
using AppCore.Interfaces;
using AppCore.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Tasks.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectController : Controller
{
    private readonly IProject _project;

    public ProjectController(IProject project)
    {
        _project = project;
    }

    [HttpGet("Projects")]
    public async Task<IActionResult> GetAllAsync()
    {
        return await _project.GetAllAsync();
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProjectAsync(int id)
    {
        try
        {
            return await _project.FindAsync(id);
        }
        catch (NotFoundException e)
        {
            return new NotFoundObjectResult(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(ProjectViewModel model)
    {
        return await _project.AddAsync(model);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> EditAsync(ProjectEditViewModel model, int id)
    {
        try
        {
            return await _project.EditAsync(model, id);
        }
        catch (NotFoundException e)
        {
            return new NotFoundObjectResult(e.Message);
        }
    }

    [HttpPut("ChangeState/{id:int}")]
    public async Task<IActionResult> ChangeStateAsync(ProjectStateViewModel model, int id)
    {
        try
        {
            return await _project.ChangeStateAsync(model, id);
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
            return await _project.RemoveAsync(id);
        }
        catch (NotFoundException e)
        {
            return new NotFoundObjectResult(e.Message);
        }
    }

    [HttpDelete("DeleteTask/{projectId:int}")]
    public async Task<IActionResult> DeleteAsync(int projectId, int taskId)
    {
        try
        {
            return await _project.RemoveTaskAsync(projectId, taskId);
        }
        catch (NotFoundException e)
        {
            return new NotFoundObjectResult(e.Message);
        }
    }
}