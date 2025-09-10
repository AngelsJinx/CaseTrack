using CaseTrack.Data.Entities;
using CaseTrack.Data.Repositories;
using CaseTrack.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CaseTrack.Modules;

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
    /// <returns>Task persisted to the database.</returns>
    /// <exception cref="InvalidOperationException">Raised when the <see cref="dto"/> already has an ID populated. Use <see cref="UpdateTask"/> to update existing tasks.</exception>
    public async Task<TaskDto> InsertTask(TaskDto dto)
    {
        if (dto.Id is not null) throw new InvalidOperationException("Cannot insert task with specific ID");

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
        return ToTaskDto(taskEntity.Entity);
    }

    /// <summary>
    /// Update the specified task.
    /// </summary>
    /// <param name="dto">Updated details.</param>
    /// <returns>Updated task.</returns>
    /// <exception cref="InvalidOperationException">Raised when the <see cref="dto"/> doesn't specify the ID of an existing task. Use <see cref="InsertTask"/> to create a new task.</exception>
    /// <exception cref="KeyNotFoundException">The unique identifier does not match an existing task.</exception>
    public async Task<TaskDto> UpdateTask(TaskDto dto)
    {
        if (dto.Id is null) throw new InvalidOperationException("ID must be specified.");
        
        var existing = await _taskRepository.Get(dto.Id.Value);
        if (existing == null) throw new KeyNotFoundException("Task not found");
        
        // Update the subset of fields that users can change.
        // In a more realistic example this could be further restricted by permissions (eg, a specific permission required to update DueDate)
        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.DueDate = dto.DueDate;
        existing.Status = dto.Status;
        
        await _taskRepository.SaveChanges();
        
        return ToTaskDto(existing);
    }

    /// <summary>
    /// Delete the specified task.
    /// </summary>
    /// <param name="id">Unique identifier for the Task.</param>
    /// <exception cref="KeyNotFoundException">The unique identifier does not match an existing task.</exception>
    public async Task DeleteTask(long id)
    {
        var task = await _taskRepository.Get(id);
        if (task is null) throw new KeyNotFoundException("Task not found");
        
        _taskRepository.Remove(task);
        await _taskRepository.SaveChanges();
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