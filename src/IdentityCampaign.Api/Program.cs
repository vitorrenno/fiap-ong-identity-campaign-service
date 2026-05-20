using FluentValidation;
using IdentityCampaign.Api.Utils;
using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaign;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaigns;
using IdentityCampaign.Application.Features.Campaigns.DeleteCampaign;
using IdentityCampaign.Application.Features.Campaigns.GetAllCampaign;
using IdentityCampaign.Application.Features.Campaigns.GetCampaignById;
using IdentityCampaign.Application.Features.Campaigns.UpdateCampaign;
using IdentityCampaign.Application.Features.Donation.CreateDonation;
using IdentityCampaign.Application.Features.Donation.GetAllDonation;
using IdentityCampaign.Application.Features.Donation.GetDonationById;
using IdentityCampaign.Application.Features.Donation.GetDonationMe;
using IdentityCampaign.Application.MapperProfile;
using IdentityCampaign.Infrastructure.Persistence;
using IdentityCampaign.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdentityCampaignDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

#region Interfaces
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<IDonationRepository, DonationRepository>();
#endregion

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

#region MediatR
//Campaign
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCampaignCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllCampaignCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIdCampaignCommandHandler).Assembly));
//Donation
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDonationCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetDonationByIdCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllDonationCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetDonationMeCommandHandler).Assembly));

#endregion

#region AutoMapper
builder.Services.AddAutoMapper(typeof(CampaignProfile));
builder.Services.AddAutoMapper(typeof(DonationProfile));
#endregion

#region Validators
builder.Services.AddScoped<IValidator<CreateCampaignCommand>, CreateCampaignCommandValidator>();
builder.Services.AddScoped<IValidator<GetAllCampaignCommand>, GetAllCampaignCommandValidator>();
builder.Services.AddScoped<IValidator<GetByIdCampaignCommand>, GetByIdCampaignCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateCampaignCommand>, UpdateCampaignCommandValidator>();
builder.Services.AddScoped<IValidator<DeleteCampaignCommand>, DeleteCampaignCommandValidator>();
//Donation
builder.Services.AddScoped<IValidator<CreateDonationCommand>, CreateDonationValidator>();
builder.Services.AddScoped<IValidator<GetDonationByIdCommand>, GetDonationByIdValidator>();
builder.Services.AddScoped<IValidator<GetAllDonationCommand>, GetAllDonationValidator>();
builder.Services.AddScoped<IValidator<GetDonationMeCommand>, GetDonationMeValidator>();

#endregion


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

#region Banco de dados
var environment = builder.Environment.EnvironmentName;

if (environment == Environments.Development)
{
    string connString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

    // Apenas para DEV LOCAL (fora do K8s)
    try
    {
        await Api.Services.DockerMySqlManager.EnsureMySqlContainerRunningAsync(connString);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Aviso: Não foi possível gerenciar container Docker. Se está em K8s, isso é esperado. Erro: {ex.Message}");
    }

    await IdentityCampaign.Infrastructure.MigrationHelper.WaitForMySqlAsync(connString);
    IdentityCampaign.Infrastructure.MigrationHelper.ApplyMigrations(app);
}
else
{
    // Kubernetes / Staging / Production
    IdentityCampaign.Infrastructure.MigrationHelper.ApplyMigrations(app);
}
#endregion

app.Run();
