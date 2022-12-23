using System.ComponentModel.DataAnnotations;
using AppCore.Enums;

namespace AppCore.ViewModel;

public class TaskEditViewModel : BaseViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
}