using AppCore.Enums;

namespace AppCore.Entities;

public class Project : BaseEntity
{
    public Project(string name,string description, Priority priority)
    {
        Name = name;
        CreatedAt = DateTime.Now;
        State = ProjectState.NotStarted;
        Priority = priority;
        Description = description;
    }
    
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletionDate { get; private set; }
    public ProjectState State { get; private set; }
    public Priority Priority { get; private set; }
    public List<Task> Tasks { get; private set; } = new();

    public Project AddTask(Task task)
    {
        Tasks.Add(task);
        return this;
    }   
    
    public Project RemoveTask(Task task)
    {
        Tasks.Remove(task);
        return this;
    }
    public Project Edit(string name, string description)
    {
        if (!string.IsNullOrEmpty(name))
            Name = name; 
        if (!string.IsNullOrEmpty(description))
            Description = description;      
        return this;
    }

    public Project ChangeState(ProjectState state)
    {
        if (ProjectState.Completed == state)
        {
            CompletionDate = DateTime.Now;
        }
        State = state;
        return this;
    }
}