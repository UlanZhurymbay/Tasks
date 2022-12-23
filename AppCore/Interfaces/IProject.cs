using AppCore.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AppCore.Interfaces;

public interface IProject : IContext
{
    public Task<IActionResult> AddAsync(ProjectViewModel model);
    public Task<IActionResult>  EditAsync(ProjectEditViewModel model, int id);
    public Task<IActionResult> ChangeStateAsync(ProjectStateViewModel model, int id);
    public Task<IActionResult> RemoveTaskAsync(int taskId, int id);

}