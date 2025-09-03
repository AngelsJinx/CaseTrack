namespace CaseTrack.DTOs;

public record ApiResponseDto<T>(bool Success, T? Payload, string? Message);