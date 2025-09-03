using System.ComponentModel.DataAnnotations;

namespace CaseTrack.Data.Entities;

/// <summary>
/// Possible statuses for a Task.
/// </summary>
/// <remarks>Any new statuses should be added to the end! Re-ordering enums changes their backed values.</remarks>
public enum TaskStatus
{
    /// <summary>
    /// Not yet started.
    /// </summary>
    Pending,
    
    /// <summary>
    /// In progress.
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Work has been (temporarily) suspended.
    /// </summary>
    OnHold,
    
    /// <summary>
    /// Task finished.
    /// </summary>
    Completed
}

/// <summary>
/// Represents a single task for a Case Worker.
/// </summary>
public class CaseTrackTask : BaseEntity
{
    // TODO Reference a Case entity (Many tasks to 1 case)
    // TODO Reference the Task assignee
    
    [MaxLength(250)] // The technical test repository doesn't specify a length requirement, so this is best guess.
    public string Title { get; set; } = null!;
    
    // For the tech test I'm leaving this full length, and suppressing the IDE warning about potential performance issues.
    // In the real world thought should be given to length restrictions (if any), and query patterns.
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string? Description { get; set; }
    
    /// <summary>
    /// The current status of the task.
    /// Defaults to <see cref="TaskStatus.Pending"/>
    /// </summary>
    public TaskStatus Status { get; set; }
    
    /// <summary>
    /// When the task is due to be completed.
    /// </summary>
    /// <remarks>Timezone-aware</remarks>
    public DateTimeOffset DueDate { get; set; }
}