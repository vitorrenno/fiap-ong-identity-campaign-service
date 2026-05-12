using AutoMapper;
using IdentityCampaign.Application.DTOs.Campaigns;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaign;
using IdentityCampaign.Application.Features.Campaigns.GetAllCampaign;
using IdentityCampaign.Application.Features.Campaigns.GetCampaignById;
using IdentityCampaign.Application.Features.Campaigns.UpdateCampaign;
using IdentityCampaign.Domain.Entities;
using IdentityCampaign.Domain.Enums;

namespace IdentityCampaign.Application.MapperProfile
{
    public class CampaignProfile : Profile
    {
        public CampaignProfile()
        {
            CreateMap<Campaign, CreateCampaignCommandResponse>();
            CreateMap<Campaign, GetAllCampaignCommandResponse>();
            CreateMap<Campaign, GetByIdCampaignCommandResponse>();
            CreateMap<Campaign, UpdateCampaignCommandResponse>();
            CreateMap<CreateCampaignRequest, CreateCampaignCommand>();
            CreateMap<UpdateCampaignRequest, UpdateCampaignCommand>();

            CreateMap<CreateCampaignCommand, Campaign>();
        }
    }
}
