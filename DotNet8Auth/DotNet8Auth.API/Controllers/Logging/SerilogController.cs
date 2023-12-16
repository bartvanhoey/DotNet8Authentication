using DotNet8Auth.Shared.Models.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNet8Auth.API.Controllers.Logging
{
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
                if (model.Level == "warning")
                {
                    logger.LogWarning($"{nameof(CreateLogEntry)}: {model.Message}");
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
}