using CaseTrack.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CaseTrack.Controllers;

public abstract class BaseController : ControllerBase
{
    protected ActionResult<ApiResponseDto<T>> ToApiResponse<T>(T? data, string? message = null)
    {
        return Ok(new ApiResponseDto<T>(true, data, message));
    }

    protected ActionResult<ApiResponseDto<T>> ToApiError<T>(T? data, string? message = null, int statusCode = 400)
    {
        return StatusCode(statusCode, new ApiResponseDto<T>(false, data, message));
    }
}