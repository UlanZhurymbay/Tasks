using AppCore.Enums;

namespace AppCore.Entities;

public class Task : BaseEntity
{
    public Task(string name, string description, Priority priority)
    {
        Name = name;
        Description = description;
        Priority = priority;
        State = TaskState.ToDo;
        CreatedAt = DateTime.Now;
    }
    
    public string Name { get; private set; }
    public string Description { get; private set; }
    public TaskState State { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletionDate { get; private set; }
    public Priority Priority { get; private set; }

    public Task Edit(string name, string description)
    {
        if (!string.IsNullOrEmpty(name))
            Name = name; 
        if (!string.IsNullOrEmpty(description))
            Description = description;     
        return this;
    }
    public Task ChangeState(TaskState state)
    {
        if (TaskState.Done == state)
        {
            CompletionDate = DateTime.Now;
        }
        State = state;
        return this;
    }
    
}