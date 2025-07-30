using System.Collections;
using Infotecs.Internship.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.Internship.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProcessingController : ControllerBase
{

    private readonly ILogger<ProcessingController> _logger;

    public ProcessingController(ILogger<ProcessingController> logger)
    {
        _logger = logger;
    }

    [HttpPost("values")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("results")]
    public async Task<ActionResult<IEnumerable<Result>>> GetResults(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("values")]
    public async Task<ActionResult<IEnumerable<Value>>> GetValues(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}