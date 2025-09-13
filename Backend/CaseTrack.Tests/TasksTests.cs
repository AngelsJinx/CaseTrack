using System.Net;
using System.Net.Http.Json;
using CaseTrack.DTOs;
using TaskStatus = CaseTrack.Data.Entities.TaskStatus;

namespace CaseTrack.Tests;

public class TasksTests : BaseTest, IClassFixture<CaseTrackWebAppFactory>
{
    // Use IClassFixture to populate the factory. This will share a single database testcontainer with all other tests using the IClassFixture.
    public TasksTests(CaseTrackWebAppFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData(TaskStatus.Pending)]
    [InlineData(TaskStatus.OnHold)]
    [InlineData(TaskStatus.InProgress)]
    [InlineData(TaskStatus.Completed)] // TODO prevent tasks being created at the Completed status!
    public async Task CreateTaskWithStatus(TaskStatus status)
    {
        var createRequestDto = new TaskDto(null, "Hello world", "This is my test task, to see if I can create any.", status, DateTimeOffset.UtcNow.AddDays(7));
        var createdTask = await Post(createRequestDto);
        Assert.NotNull(createdTask);

        Assert.NotNull(createdTask.Id);
        Assert.Equal(status, createdTask.Status);
        Assert.Equal(createRequestDto.Title, createdTask.Title);
        Assert.Equal(createRequestDto.Description, createdTask.Description);
        
        // Ensure the task was created recently!
        Assert.True(DateTimeOffset.UtcNow.AddSeconds(-3) < createRequestDto.DueDate);
    }

    [Theory]
    [InlineData(TaskStatus.Pending)]
    [InlineData(TaskStatus.OnHold)]
    [InlineData(TaskStatus.InProgress)]
    [InlineData(TaskStatus.Completed)]
    public async Task UpdateTaskStatus(TaskStatus newStatus) // Ensure we can transition from Pending to any of the other statuses
    {
        var createDto = new TaskDto(null, "Update test", "Here's a task we'll repeatedly change the status of", TaskStatus.Pending, DateTimeOffset.UtcNow.AddDays(7));
        var createdDto = await Post(createDto);

        var updatedDto = await Post(createdDto with
        {
            Status = newStatus
        });
        
        Assert.NotNull(updatedDto);
        Assert.Equal(newStatus, updatedDto.Status);
    }

    [Fact]
    public async Task UpdateTaskTitle()
    {
        var createDto = new TaskDto(null, "A title to update", "Here's a task we'll rename", TaskStatus.Pending, DateTimeOffset.UtcNow.AddDays(7));
        var createdDto = await Post(createDto);
        
        Assert.Equal(createdDto.Title, createDto.Title);

        var updatedDto = await Post(createdDto with
        {
            Title = "Updated title"
        });
        
        Assert.NotNull(updatedDto);
        Assert.Equal("Updated title", updatedDto.Title);
    }
    
    [Fact]
    public async Task UpdateTaskDescription()
    {
        var createDto = new TaskDto(null, "A title to keep", "A description to change", TaskStatus.Pending, DateTimeOffset.UtcNow.AddDays(7));
        var createdDto = await Post(createDto);
        
        Assert.Equal(createdDto.Description, createDto.Description);

        var updatedDto = await Post(createdDto with
        {
            Description = "Updated description"
        });
        
        Assert.NotNull(updatedDto);
        Assert.Equal("Updated description", updatedDto.Description);
    }
    
    [Fact]
    public async Task InsertTaskDueDateInPast()
    {
        var createDto = new TaskDto(null, "A task to break", "Trying to set the due date to the past should fail as a bad request.", TaskStatus.Pending, DateTimeOffset.UtcNow.AddDays(-7));
        var response = await Client.PostAsJsonAsync("api/Task", createDto);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseData = await response.Content.ReadFromJsonAsync<ApiResponseDto<TaskDto>>();
        Assert.NotNull(responseData);
        Assert.False(responseData.Success);
        Assert.Equal("Due date cannot be in the past.", responseData.Message);
    }
    
    [Fact]
    public async Task UpdateTaskDueDateToPast()
    {
        var createDto = new TaskDto(null, "A task to break", "Trying to set the due date to the past should fail as a bad request.", TaskStatus.Pending, DateTimeOffset.UtcNow.AddDays(7));
        var createdDto = await Post(createDto);
        
        Assert.Equal(createdDto.DueDate, createDto.DueDate);

        var response = await Client.PostAsJsonAsync("api/Task", createdDto with
        {
            DueDate = DateTimeOffset.UtcNow.AddDays(-3)
        });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
        var responseData = await response.Content.ReadFromJsonAsync<ApiResponseDto<TaskDto>>();
        Assert.NotNull(responseData);
        Assert.False(responseData.Success);
        Assert.Equal("Due date cannot be in the past.", responseData.Message);
    }

    private async Task<TaskDto> Post(TaskDto dto)
    {
        var response = await Client.PostAsJsonAsync("api/Task", dto);
        response.EnsureSuccessStatusCode();
        
        var createdTaskResponse = await response.Content.ReadFromJsonAsync<ApiResponseDto<TaskDto>>();
        Assert.NotNull(createdTaskResponse);
        Assert.NotNull(createdTaskResponse.Payload);
        
        return createdTaskResponse.Payload;
    }
}