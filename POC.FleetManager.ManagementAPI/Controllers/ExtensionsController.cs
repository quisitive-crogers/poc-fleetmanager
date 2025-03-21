using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace POC.FleetManager.ManagementAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExtensionsController(IConfiguration configuration) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetExtensions(string framework)
        {
            var userName = User.Identity!.Name;

            // Get allowed extensions from appsettings.json
            var extensionsConfig = configuration.GetSection("Extensions").Get<Dictionary<string, ExtensionConfig>>();
            var extensions = extensionsConfig!
               .Where(ext => ext.Value.AllowedUsers.Contains("*") || ext.Value.AllowedUsers.Contains(userName!))
               .Select(ext => new
               {
                   Id = ext.Key,
                   Url = $"/api/extensions/{ext.Key}/{framework}/{ext.Key}.dll" // URL to download the DLL
               })
               .ToList();


            return Ok(JsonConvert.SerializeObject(new { Extensions = extensions, UserName = userName }));
        }

        [HttpGet("{extensionName}/{framework}/{fileName}.dll")]
        public IActionResult GetExtensionBinary(string extensionName, string framework, string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "extensions", extensionName, framework, $"{fileName}.dll");
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound($"Extension DLL not found: {fileName}.dll for framework {framework}");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", $"{fileName}.dll");
        }
    }

    public class ExtensionConfig
    {
        public required List<string> AllowedUsers { get; set; }
    }
}
