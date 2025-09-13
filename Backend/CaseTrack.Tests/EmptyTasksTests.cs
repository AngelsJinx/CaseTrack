using System.Net;
using System.Net.Http.Json;
using CaseTrack.DTOs;

namespace CaseTrack.Tests;

public class EmptyTasksTests : BaseTest // Not taking the IClassFixture, so this class is guaranteed a fresh database container
{
    [Fact]
    public async Task GetAll()
    {
        var tasksResponse = await Client.GetAsync("api/Task");
        tasksResponse.EnsureSuccessStatusCode();

        var tasks = await tasksResponse.Content.ReadFromJsonAsync<ApiResponseDto<IEnumerable<Task>>>();
        Assert.NotNull(tasks);
        Assert.True(tasks.Success);
        Assert.NotNull(tasks.Payload);
        Assert.Empty(tasks.Payload);
    }

    [Fact]
    public async Task DeleteByUnknownIdFails()
    {
        var deleteResponse = await Client.DeleteAsync($"api/Task/99928347");
        Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
    }
}