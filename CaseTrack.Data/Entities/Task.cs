namespace CaseTrack.Data.Entities;

public enum TaskStatus
{
    Pending = 0,
    Started,
    OnHold,
    Completed
}

public class Task
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public DateTimeOffset DueDate { get; set; }
}