using System.ComponentModel.DataAnnotations;
using AppCore.Enums;

namespace AppCore.ViewModel;

public class ProjectViewModel : BaseViewModel
{
    [Required]
    public string Name { get;  set; }
    [Required]
    public string Description { get; set; }
    [Range(1, 3)]
    public Priority Priority { get;  set; }
}