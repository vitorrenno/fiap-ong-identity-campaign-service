using FluentAssertions;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaigns;
using IdentityCampaign.IntegrationTests.Commands.Campaign;

namespace IdentityCampaign.UnitTests.Campaign
{
    public class CreateCampaignCommandValidatorTest
    {
        private readonly CreateCampaignCommandValidator _validator = new();

        [Fact]
        public void ValidarComando_Valido_DeveRetornarSucesso()
        {
            var request = CreateCampaignCommandBuilder.Build();
            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Deve_falhar_quando_titulo_esta_vazio()
        {
            var request = CreateCampaignCommandBuilder.Build() with { Title = string.Empty };
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage == "O título da campanha é obrigatório.");
        }

        [Fact]
        public void Deve_falhar_quando_titulo_tem_menos_de_3_caracteres()
        {
            var request = CreateCampaignCommandBuilder.Build() with { Title = "AB" };
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage == "O título da campanha deve ter entre 3 e 200 caracteres.");
        }

        [Fact]
        public void Deve_falhar_quando_titulo_tem_mais_de_200_caracteres()
        {
            var request = CreateCampaignCommandBuilder.Build() with { Title = new string('A', 201) };
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage == "O título da campanha deve ter entre 3 e 200 caracteres.");
        }

        [Fact]
        public void Deve_falhar_quando_descricao_esta_vazia()
        {
            var request = CreateCampaignCommandBuilder.Build() with { Description = string.Empty };
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Description" && e.ErrorMessage == "A descrição da campanha é obrigatória.");
        }

        [Fact]
        public void Deve_falhar_quando_descricao_tem_menos_de_10_caracteres()
        {
            var request = CreateCampaignCommandBuilder.Build() with { Description = "Curto" };
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Description" && e.ErrorMessage == "A descrição da campanha deve ter entre 10 e 1000 caracteres.");
        }

        [Fact]
        public void Deve_falhar_quando_descricao_tem_mais_de_1000_caracteres()
        {
            var request = CreateCampaignCommandBuilder.Build() with { Description = new string('D', 1001) };
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Description" && e.ErrorMessage == "A descrição da campanha deve ter entre 10 e 1000 caracteres.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        [InlineData(-100.50)]
        public void Deve_falhar_quando_meta_financeira_e_zero_ou_negativa(decimal goalAmount)
        {
            var request = CreateCampaignCommandBuilder.Build() with { GoalAmount = goalAmount };
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "GoalAmount" && e.ErrorMessage == "A meta financeira deve ser maior que zero.");
        }

        [Fact]
        public void Deve_falhar_quando_data_termino_esta_no_passado()
        {
            var request = CreateCampaignCommandBuilder.Build() with
            {
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(-5)
            };
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "EndDate" && e.ErrorMessage == "A data de término não pode estar no passado.");
        }

        [Fact]
        public void Deve_falhar_quando_data_termino_e_anterior_a_data_inicio()
        {
            var request = CreateCampaignCommandBuilder.Build() with
            {
                StartDate = DateTime.Now.AddDays(10),
                EndDate = DateTime.Now.AddDays(5)
            };
            var result = _validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "EndDate" && e.ErrorMessage == "A data de término deve ser posterior à data de início.");
        }

        [Fact]
        public void Deve_passar_quando_data_termino_e_posterior_a_data_inicio()
        {
            var request = CreateCampaignCommandBuilder.Build() with
            {
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(30)
            };
            var result = _validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }
    }
}
