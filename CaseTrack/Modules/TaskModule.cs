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
    /// Convert a internal task representation into a DTO that can be returned to the front-end.
    /// </summary>
    /// <param name="task">The task to convert.</param>
    /// <returns>A sharable representation of the task.</returns>
    /// <remarks>This ensures only relevant parts of the internal representation are returned to the front-end</remarks>
    private TaskDto ToTaskDto(CaseTrackTask task)
    {
        // In a more complex scenario this could be handled through a mapper class.
        // This could be injected to the module through DI, along with the repositories.
        return new TaskDto(task.Title, task.Description, task.Status, task.DueDate);
    }
}