using Microsoft.AspNetCore.Mvc;

namespace Server.Core.Controllers;

public class HealthController : BaseApiController
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        return Ok("Healthy");
    }
}