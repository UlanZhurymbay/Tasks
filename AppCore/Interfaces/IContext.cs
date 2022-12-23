using Microsoft.AspNetCore.Mvc;

namespace AppCore.Interfaces;

public interface IContext
{
    public Task<IActionResult>  GetAllAsync();
    public Task<IActionResult>  FindAsync(int id);
    public Task<IActionResult>  RemoveAsync(int id);
}