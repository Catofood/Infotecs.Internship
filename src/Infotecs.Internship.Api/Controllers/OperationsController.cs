using AutoMapper;
using Infotecs.Internship.Api.Dto;
using Infotecs.Internship.Application.Handlers.GetResultsQuery;
using Infotecs.Internship.Application.Handlers.GetValuesQuery;
using Infotecs.Internship.Application.Handlers.UploadFileCommand;
using Infotecs.Internship.Application.Pagination;
using Infotecs.Internship.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.Internship.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public OperationsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("files")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFile([FromForm] FileFormDto formDto, CancellationToken cancellationToken)
    {
        var file = formDto.File;
        var command = new UploadFileCommand()
        {
            FileName = file.FileName,
            FileContentStream = file.OpenReadStream(),
        };
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet("results")]
    //TODO: Решить проблему невозврата данных о странице на клиент
    public async Task<ActionResult<PaginatedList<OperationsResultWithFileNameDto>?>> GetResults([FromQuery] GetResultsQuery query, CancellationToken cancellationToken)
    {
        var pagedResults = await _mediator.Send(query, cancellationToken);
        var pagedDto = _mapper.Map<PaginatedList<OperationsResultWithFileNameDto>>(pagedResults);
        return Ok(pagedDto);
    }

    [HttpGet("values")]
    public async Task<ActionResult<PaginatedList<OperationValuesWithFileNameDto>>> GetValues([FromQuery] GetValuesQuery query, CancellationToken cancellationToken)
    {
        var pagedValues = await _mediator.Send(query, cancellationToken);
        var pagedDto = _mapper.Map<PaginatedList<OperationValuesWithFileNameDto>>(pagedValues);
        return Ok(pagedDto);
    }
}