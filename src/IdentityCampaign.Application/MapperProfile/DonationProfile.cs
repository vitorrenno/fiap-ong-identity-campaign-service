using AutoMapper;
using IdentityCampaign.Application.DTOs.Donation;
using IdentityCampaign.Application.Features.Donation.CreateDonation;
using IdentityCampaign.Domain.Entities;

namespace IdentityCampaign.Application.MapperProfile
{
    public class DonationProfile : Profile
    {
        public DonationProfile()
        {
            //CREATE DONATION
            CreateMap<CreateDonation, CreateDonationCommand>();
            CreateMap<CreateDonationCommand,CreateDonationResponse>();
            CreateMap<Donation, CreateDonationResponse>();
            //
        }

    }
}
