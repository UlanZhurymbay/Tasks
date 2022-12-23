using System.ComponentModel.DataAnnotations;
using AppCore.Enums;

namespace AppCore.ViewModel;

public class TaskStateViewModel : BaseViewModel
{
    [Range(1, 3)]
    public TaskState State { get; set; }
}