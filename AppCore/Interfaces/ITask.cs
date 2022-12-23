using AppCore.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AppCore.Interfaces;

public interface ITask : IContext
{
    public Task<IActionResult> AddAsync(TaskViewModel model);
    public Task<IActionResult> EditAsync(TaskEditViewModel model, int id);
    public Task<IActionResult> ChangeStateAsync(TaskStateViewModel model, int id);
}