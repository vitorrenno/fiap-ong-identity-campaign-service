using AutoMapper;
using IdentityCampaign.Application.Common;
using IdentityCampaign.Application.DTOs.Donation;
using IdentityCampaign.Application.Features.Donation.CreateDonation;
using IdentityCampaign.Application.Features.Donation.GetAllDonation;
using IdentityCampaign.Application.Features.Donation.GetDonationById;
using IdentityCampaign.Application.Features.Donation.GetDonationMe;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityCampaign.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
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
        [Authorize(Roles = RoleConstants.Donor)]
        public async Task<IActionResult> Create([FromBody] CreateDonation request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CreateDonationCommand>(request);
            var response = await _mediator.Send(command, cancellationToken);
            return Created(string.Empty, response);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<GetDonationByIdCommand>(id);
            var donation = await _mediator.Send(command);

            if (donation is null)
                return NotFound();

            return Ok(donation);
        }
        [HttpGet]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllDonationCommand();
            var campaigns = await _mediator.Send(query);
            return Ok(campaigns);
        }
        [HttpGet("GetDonationByUser/{idUser}")]
        [Authorize(Roles = RoleConstants.Donor)]
        public async Task<IActionResult> GetDonationByUser(Guid idUser, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<GetDonationMeCommand>(idUser);
            var donations = await _mediator.Send(command);

            if (donations is null)
                return NotFound();

            return Ok(donations);
        }

    }
}
