using FluentAssertions;
using FluentValidation.Results;
using IdentityCampaign.Application.Features.Campaigns.UpdateCampaign;
using IdentityCampaign.IntegrationTests.Commands.Campaign;

namespace IdentityCampaign.UnitTests.Campaign;

public class UpdateCampaignCommandValidatorTest
{
    private readonly UpdateCampaignCommandValidator _validator = new();

    [Fact]
    public void ValidarComando_Valido_DeveRetornarSucesso()
    {
        UpdateCampaignCommand request = UpdateCampaignCommandBuilder.Build();
        ValidationResult result = _validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Deve_falhar_quando_id_esta_vazio()
    {
        UpdateCampaignCommand request = UpdateCampaignCommandBuilder.Build() with { Id = Guid.Empty };
        ValidationResult result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Deve_falhar_quando_titulo_esta_vazio()
    {
        UpdateCampaignCommand request = UpdateCampaignCommandBuilder.Build() with { Title = string.Empty };
        ValidationResult result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Deve_falhar_quando_descricao_esta_vazia()
    {
        UpdateCampaignCommand request = UpdateCampaignCommandBuilder.Build() with { Description = string.Empty };
        ValidationResult result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Deve_falhar_quando_meta_financeira_eh_menor_ou_igual_a_zero()
    {
        UpdateCampaignCommand request = UpdateCampaignCommandBuilder.Build() with { GoalAmount = 0 };
        ValidationResult result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Deve_falhar_quando_data_final_eh_menor_que_data_inicial()
    {
        UpdateCampaignCommand request = UpdateCampaignCommandBuilder.Build() with
        {
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(-1)
        };

        ValidationResult result = _validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
