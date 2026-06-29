using Microsoft.AspNetCore.Mvc;
using PunchSync.Domain.Common;

namespace PunchSync.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
        => result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(new { error = result.Error, code = result.ErrorCode });

    protected IActionResult HandleResult(Result result)
        => result.IsSuccess
            ? Ok()
            : BadRequest(new { error = result.Error, code = result.ErrorCode });

    protected IActionResult HandleCreated<T>(Result<T> result, string actionName, object routeValues)
        => result.IsSuccess
            ? CreatedAtAction(actionName, routeValues, result.Value)
            : BadRequest(new { error = result.Error, code = result.ErrorCode });
}
