using AutoMapper;
using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Application.DTOs.Campaigns;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaign;
using IdentityCampaign.Application.Features.Campaigns.GetAllCampaign;
using IdentityCampaign.Application.Features.Campaigns.GetCampaignById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityCampaign.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CampaignsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllCampaignCommand();
        var campaigns = await _mediator.Send(query);
        return Ok(campaigns);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<GetByIdCampaignCommand>(id);
        var campaign = await _mediator.Send(command);

        if (campaign is null)
            return NotFound();

        return Ok(campaign);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCampaignRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateCampaignCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, response);
    }
}
