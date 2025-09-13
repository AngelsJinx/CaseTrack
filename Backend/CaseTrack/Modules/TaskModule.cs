using CaseTrack.Data.Entities;
using CaseTrack.Data.Repositories;
using CaseTrack.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CaseTrack.Modules;

public enum TaskActionStatus
{
    NotFound,
    Success,
    ValidationError
}

public record TaskActionResult(TaskActionStatus TaskActionStatus, string? Message, TaskDto? Task);

/// <summary>
/// Module for business-logic relating to Tasks.
/// </summary>
/// <param name="taskRepository">Database repository for managing Tasks.</param>
/// <remarks>
/// Modules act as an intermediary between the controller methods & persistence layer.
/// They could build on several repositories (ie, Cases and Tasks) to implement the core business logic.
/// </remarks>
public class TaskModule(IRepository<CaseTrackTask> taskRepository)
{
    private readonly IRepository<CaseTrackTask> _taskRepository = taskRepository;

    /// <summary>
    /// Retrieve all existing tasks.
    /// </summary>
    /// <returns>DTOs representing each Task in the system.</returns>
    public async Task<IEnumerable<TaskDto>> GetTasks()
    {
        // This could be extended to take filtering parameters, to filter our queryable to particular statuses etc.
        
        var allTasks = await _taskRepository.GetQueryable().ToListAsync();
        return allTasks.Select(ToTaskDto);
    }

    /// <summary>
    /// Get a specific task.
    /// </summary>
    /// <param name="id">Unique identifier of the task.</param>
    /// <returns>The specified task, or null if it doesn't exist.</returns>
    public async Task<TaskDto?> GetTask(long id)
    {
        var task = await _taskRepository.Get(id);
        return task is null ? null : ToTaskDto(task);
    }

    /// <summary>
    /// Create a new task and persist it to the database.
    /// </summary>
    /// <param name="dto">Task details</param>
    /// <returns>Result object containing the inserted task details, or error details if unsuccessful.</returns>
    public async Task<TaskActionResult> InsertTask(TaskDto dto)
    {
        if (dto.Id is not null)
        {
            return new TaskActionResult(TaskActionStatus.ValidationError, "ID cannot be specified for inserts.", null);
        }
        // DueDate needs to be in the future, but we'll allow a small grace period for anyone setting a due date to the 'current time' on a slow connection
        if (dto.DueDate < DateTimeOffset.UtcNow.AddSeconds(10))
        {
            return new TaskActionResult(TaskActionStatus.ValidationError, "Due date cannot be in the past.", null);
        }

        var newTask = new CaseTrackTask()
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Status = dto.Status,
        };
        var taskEntity = await _taskRepository.Add(newTask);
        await _taskRepository.SaveChanges();

        // Returning the EntityEntry value as EF will have populated Created/Id while saving.
        return new TaskActionResult(TaskActionStatus.Success, null, ToTaskDto(taskEntity.Entity));
    }

    /// <summary>
    /// Update the specified task.
    /// </summary>
    /// <param name="dto">Updated details.</param>
    /// <returns>Result object containing the updated task details, or error details if unsuccessful.</returns>
    public async Task<TaskActionResult> UpdateTask(TaskDto dto)
    {
        if (dto.Id is null)
        {
            return new TaskActionResult(TaskActionStatus.ValidationError, "ID must be specified.", null);
        }
        // DueDate needs to be in the future, but we'll allow a small grace period for anyone setting a due date to the 'current time' on a slow connection
        if (dto.DueDate < DateTimeOffset.UtcNow.AddSeconds(10))
        {
            return new TaskActionResult(TaskActionStatus.ValidationError, "Due date cannot be in the past.", null);
        }
        
        var existing = await _taskRepository.Get(dto.Id.Value);
        if (existing == null)
        {
            return new TaskActionResult(TaskActionStatus.NotFound, "Task not found.", null);
        }
        
        // Update the subset of fields that users can change.
        // In a more realistic example this could be further restricted by permissions (eg, a specific permission required to update DueDate)
        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.DueDate = dto.DueDate;
        existing.Status = dto.Status;
        
        await _taskRepository.SaveChanges();
        
        return new TaskActionResult(TaskActionStatus.Success, null, ToTaskDto(existing));
    }

    /// <summary>
    /// Delete the specified task.
    /// </summary>
    /// <param name="id">Unique identifier for the Task.</param>
    /// <return>Result object with a success status, or error details on failure.</return>
    public async Task<TaskActionResult> DeleteTask(long id)
    {
        var task = await _taskRepository.Get(id);
        if (task is null)
        {
            return new  TaskActionResult(TaskActionStatus.NotFound, "Task not found.", null);
        }
        
        _taskRepository.Remove(task);
        await _taskRepository.SaveChanges();
        return new TaskActionResult(TaskActionStatus.Success, null, null);
    }

    /// <summary>
    /// Convert a internal task representation into a DTO that can be returned to the front-end.
    /// </summary>
    /// <param name="task">The task to convert.</param>
    /// <returns>A sharable representation of the task.</returns>
    /// <remarks>This ensures only relevant parts of the internal representation are returned to the front-end</remarks>
    private TaskDto ToTaskDto(CaseTrackTask task)
    {
        // In a more complex scenario this could be handled through a mapper class.
        // This could be injected to the module through DI, along with the repositories.
        return new TaskDto(task.Id, task.Title, task.Description, task.Status, task.DueDate);
    }
}