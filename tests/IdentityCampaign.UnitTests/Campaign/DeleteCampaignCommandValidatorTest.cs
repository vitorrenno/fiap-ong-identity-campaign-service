using FluentAssertions;
using FluentValidation.Results;
using IdentityCampaign.Application.Features.Campaigns.DeleteCampaign;
using IdentityCampaign.IntegrationTests.Commands.Campaign;

namespace IdentityCampaign.UnitTests.Campaign;

public class DeleteCampaignCommandValidatorTest
{
    private readonly DeleteCampaignCommandValidator _validator = new();

    [Fact]
    public void ValidarComando_Valido_DeveRetornarSucesso()
    {
        DeleteCampaignCommand request = DeleteCampaignCommandBuilder.Build();
        ValidationResult result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Deve_falhar_quando_id_esta_vazio()
    {
        DeleteCampaignCommand request = DeleteCampaignCommandBuilder.Build() with { Id = Guid.Empty };
        ValidationResult result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.PropertyName == "Id" &&
            e.ErrorMessage == "O ID da campanha é obrigatório.");
    }
}
