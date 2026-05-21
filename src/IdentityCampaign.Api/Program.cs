using FluentValidation;
using IdentityCampaign.Api.Monitoring;
using IdentityCampaign.Api.Monitoring.MonitoringMiddleware;
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
using IdentityCampaign.Application.Messaging.Events;
using IdentityCampaign.Infrastructure;
using IdentityCampaign.Infrastructure.Persistence;
using IdentityCampaign.Infrastructure.Repositories;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Prometheus.DotNetRuntime;

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
#endregion

#region Monitoring
builder.Services.AddSingleton<IMetricsService, MetricsService>();
builder.Services.AddScoped<IValidator<DeleteCampaignCommand>, DeleteCampaignCommandValidator>();
//Donation
builder.Services.AddScoped<IValidator<CreateDonationCommand>, CreateDonationValidator>();
builder.Services.AddScoped<IValidator<GetDonationByIdCommand>, GetDonationByIdValidator>();
builder.Services.AddScoped<IValidator<GetAllDonationCommand>, GetAllDonationValidator>();
builder.Services.AddScoped<IValidator<GetDonationMeCommand>, GetDonationMeValidator>();

#endregion

#region MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<DonationReceivedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        MassTransitConfiguration.Configure(context, cfg);
    });
});
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

DotNetRuntimeStatsBuilder.Default().StartCollecting();

app.UseRouting();

app.UseMiddleware<MonitoringMiddleware>();
app.MapMetrics();

app.MapControllers();
app.MapHealthChecks("/health");

#region Banco de dados
var environment = builder.Environment.EnvironmentName;
var connString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ========================================");
Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Iniciando aplicação em ambiente: {environment}");
Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ========================================");

if (environment == Environments.Development)
{
    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Modo DESENVOLVIMENTO detectado.");

    // Apenas para DEV LOCAL (fora do K8s)
    try
    {
        await Api.Services.DockerMySqlManager.EnsureMySqlContainerRunningAsync(connString);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Aviso: Não foi possível gerenciar container Docker. Se está em K8s, isso é esperado. Erro: {ex.Message}");
    }

    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Aguardando MySQL estar pronto...");
    await MigrationHelper.WaitForMySqlAsync(connString);

    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Aplicando migrations...");
    MigrationHelper.ApplyMigrations(app);
}
else
{
    // Kubernetes / Staging / Production
    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Modo PRODUÇÃO/KUBERNETES detectado.");
    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Aguardando MySQL estar pronto...");

    try
    {
        await MigrationHelper.WaitForMySqlAsync(connString);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERRO CRÍTICO ao conectar no MySQL: {ex.Message}");
        throw;
    }

    if (args.Contains("migrate"))
    {
        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Aplicando migrations...");
        MigrationHelper.ApplyMigrations(app);
        return;
    }
}

Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ========================================");
Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Aplicação iniciada com sucesso!");
Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ========================================");
#endregion

app.Run();
