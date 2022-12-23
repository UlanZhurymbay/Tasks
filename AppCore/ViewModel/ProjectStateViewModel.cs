using System.ComponentModel.DataAnnotations;
using AppCore.Enums;

namespace AppCore.ViewModel;

public class ProjectStateViewModel : BaseViewModel
{
    [Range(1, 3)]
    public ProjectState State { get; set; }
}