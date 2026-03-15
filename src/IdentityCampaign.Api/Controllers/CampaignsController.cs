using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Application.DTOs;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaign;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCampaign.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CampaignsController : ControllerBase
{
    private readonly ICampaignRepository _campaignRepository;
    private readonly IMediator _mediator;

    public CampaignsController(
        ICampaignRepository campaignRepository,
        IMediator mediator)
    {
        _campaignRepository = campaignRepository;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var campaigns = await _campaignRepository.GetAllAsync(cancellationToken);
        return Ok(campaigns);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var campaign = await _campaignRepository.GetByIdAsync(id, cancellationToken);

        if (campaign is null)
            return NotFound();

        return Ok(campaign);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCampaignRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCampaignCommand(
            request.Title,
            request.Description,
            request.GoalAmount,
            request.StartDate,
            request.EndDate);

        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
}