using AutoMapper;
using IdentityCampaign.Application.DTOs.Campaigns;
using IdentityCampaign.Application.DTOs.Donation;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaign;
using IdentityCampaign.Application.Features.Campaigns.GetAllCampaign;
using IdentityCampaign.Application.Features.Donation.CreateDonation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCampaign.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DonationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DonationController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDonation request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateDonationCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);
            return Created(string.Empty, response);
        }

    }
}
