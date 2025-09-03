using TaskStatus = CaseTrack.Data.Entities.TaskStatus;

namespace CaseTrack.DTOs;

public record TaskDto(string Title, string? Description, TaskStatus Status, DateTimeOffset DueDate);