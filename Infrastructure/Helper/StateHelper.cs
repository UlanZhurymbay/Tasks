using AppCore.Enums;

namespace Infrastructure.Helper;

public class StateHelper
{
    public string ChangeStateMessage(ProjectState state, ProjectState changeState)
    {
        return (state, changeState) switch
        {
            (ProjectState.NotStarted, ProjectState.NotStarted) => "State is already not started",
            (ProjectState.NotStarted, ProjectState.Active) => string.Empty,
            (ProjectState.NotStarted, ProjectState.Completed) => "State is not active",

            (ProjectState.Active, ProjectState.NotStarted) => "State was started",
            (ProjectState.Active, ProjectState.Active) => "State is already active",
            (ProjectState.Active, ProjectState.Completed) => string.Empty,

            (ProjectState.Completed, ProjectState.NotStarted) => "State was completed",
            (ProjectState.Completed, ProjectState.Active) => "State was completed",
            (ProjectState.Completed, ProjectState.Completed) => "State is already completed",
            _ => string.Empty
        };
    }
    public string ChangeStateMessage(TaskState state, TaskState changeState)
    {
        return (state, changeState) switch
        {
            (TaskState.ToDo, TaskState.ToDo) => "State is already not started",
            (TaskState.ToDo, TaskState.InProgress) => string.Empty,
            (TaskState.ToDo, TaskState.Done) => "State is not active",

            (TaskState.InProgress, TaskState.ToDo) => "State was started",
            (TaskState.InProgress, TaskState.InProgress) => "State is already active",
            (TaskState.InProgress, TaskState.Done) => string.Empty,

            (TaskState.Done, TaskState.ToDo) => "State was completed",
            (TaskState.Done, TaskState.InProgress) => "State was completed",
            (TaskState.Done, TaskState.Done) => "State is already completed",
            _ => string.Empty
        };
    }
}