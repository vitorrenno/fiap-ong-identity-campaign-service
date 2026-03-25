using FluentAssertions;
using IdentityCampaign.Application.Features.Campaigns.GetCampaignById;
using IdentityCampaign.IntegrationTests.Commands.Campaign;

namespace IdentityCampaign.UnitTests.Campaign
{
    public class GetByIdCampaignCommandValidatorTest
    {
        private readonly GetByIdCampaignCommandValidator _validator = new();

        [Fact]
        public void ValidarComando_Valido_DeveRetornarSucesso()
        {
            var request = GetByIdCampaignCommandBuilder.Build();
            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Deve_falhar_quando_id_esta_vazio()
        {
            var request = GetByIdCampaignCommandBuilder.Build() with { Id = Guid.Empty };
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Id" && e.ErrorMessage == "O ID da campanha é obrigatório.");
        }
    }
}
