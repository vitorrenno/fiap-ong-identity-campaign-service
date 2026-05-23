using AutoMapper;
using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Application.Common;
using IdentityCampaign.Application.DTOs.Campaigns;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaign;
using IdentityCampaign.Application.Features.Campaigns.GetAllCampaign;
using IdentityCampaign.Application.Features.Campaigns.GetCampaignById;
using IdentityCampaign.Application.Features.Campaigns.UpdateCampaign;
using IdentityCampaign.Application.Features.Campaigns.DeleteCampaign;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityCampaign.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllCampaignCommand();
        var campaigns = await _mediator.Send(query);
        return Ok(campaigns);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<GetByIdCampaignCommand>(id);
        var campaign = await _mediator.Send(command);

        if (campaign is null)
            return NotFound();

        return Ok(campaign);
    }

    [HttpPost]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> Create([FromBody] CreateCampaignRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateCampaignCommand>(request);
        var response = await _mediator.Send(command, cancellationToken);
        return Created(string.Empty, response);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCampaignRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCampaignCommand(
            id,
            request.Title,
            request.Description,
            request.GoalAmount,
            request.StartDate,
            request.EndDate
        );

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = RoleConstants.Admin)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteCampaignCommand(id);

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

}
