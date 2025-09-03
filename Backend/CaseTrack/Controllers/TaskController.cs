using System.Net;
using CaseTrack.DTOs;
using CaseTrack.Modules;
using Microsoft.AspNetCore.Mvc;

namespace CaseTrack.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController(TaskModule taskModule) : BaseController
{
    /// <summary>
    /// Retrieve all tasks.
    /// </summary>
    /// <returns>ApiResponse containing all available tasks.</returns>
    [HttpGet] // Controversially this is /api/task, instead of /api/tasks!
    public async Task<ActionResult<ApiResponseDto<IEnumerable<TaskDto>>>> Get()
    {
        return ToApiResponse(await taskModule.GetTasks());
    }

    /// <summary>
    /// Retrieve a specific task.
    /// </summary>
    /// <param name="id">Unique identifier for the task.</param>
    /// <returns>ApiResponse containing a task, or a NotFound response if the task doesn't exist.</returns>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ApiResponseDto<TaskDto>>> Get(long id)
    {
        var task = await taskModule.GetTask(id);
        if (task is null)
        {
            return ToApiError<TaskDto>(null, "Task not found", (int)HttpStatusCode.NotFound);
        }
        
        return ToApiResponse(task);
    }
}