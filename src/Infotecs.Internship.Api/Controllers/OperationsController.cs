using Infotecs.Internship.Api.Controllers.Dto;
using Infotecs.Internship.Application.Handlers.UploadFile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.Internship.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperationsController : ControllerBase
{

    private readonly ILogger<OperationsController> _logger;
    private readonly IMediator _mediator;

    public OperationsController(ILogger<OperationsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("files")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request, CancellationToken cancellationToken)
    {
        var file = request.File;
        var command = new UploadFileCommand()
        {
            FileName = file.FileName,
            FileContentStream = file.OpenReadStream(),
        };
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet("results")]
    public async Task<ActionResult> GetResults(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    [HttpGet("values")]
    public async Task<ActionResult> GetValues(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}