using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace POC.FleetManager.ManagementAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class HelloController : ControllerBase
    {
        [HttpGet(Name = "SayHello")]
        public string Get()
        {
            var usersName = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? User.Identity?.Name ?? "";

            return $"Hello {usersName}";
        }
    }
}
