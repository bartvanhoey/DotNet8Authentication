using DotNet8Auth.Shared.Models.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// ReSharper disable TemplateIsNotCompileTimeConstantProblem

namespace DotNet8Auth.API.Controllers.Logging;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SerilogController(ILogger<SerilogController> logger) : ControllerBase
{
    [HttpPost]
    [Route("create-log-entry")]
    public async Task<IActionResult> CreateLogEntry([FromBody] CreateLogEntryInputModel model)
    {
        try
        {
            switch (model.Level)
            {
                case "warning":
                    logger.LogWarning(model.Message);
                    break;
                case "error":
                    logger.LogError(model.Message);
                    break;
                case "critical":
                    logger.LogCritical(model.Message);
                    break;
                case "trace":
                    logger.LogTrace(model.Message);
                    break;
                case "debug":
                    logger.LogDebug(model.Message);
                    break;
            }

            await Task.CompletedTask;
                
            return Ok();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, nameof(CreateLogEntry));
            return StatusCode(StatusCodes.Status500InternalServerError,
                new SerilogResponse("Error", "Something went wrong"));
        }
    }
}