using FluentValidation;
using IdentityCampaign.Api.Utils;
using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaign;
using IdentityCampaign.Application.Features.Campaigns.CreateCampaigns;
using IdentityCampaign.Application.Features.Campaigns.GetAllCampaign;
using IdentityCampaign.Application.Features.Campaigns.GetCampaignById;
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
#endregion

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

#region MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCampaignCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllCampaignCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIdCampaignCommandHandler).Assembly));
#endregion

#region AutoMapper
builder.Services.AddAutoMapper(typeof(CampaignProfile));
#endregion

#region Validators
builder.Services.AddScoped<IValidator<CreateCampaignCommand>, CreateCampaignCommandValidator>();
builder.Services.AddScoped<IValidator<GetAllCampaignCommand>, GetAllCampaignCommandValidator>();
builder.Services.AddScoped<IValidator<GetByIdCampaignCommand>, GetByIdCampaignCommandValidator>();
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

#region Banco de dados LOCAL
string connString = builder.Configuration.GetConnectionString(name: "DefaultConnection") ?? "";
//DOCKER PRECISA ESTAR RODANDO PARA O PROJETO FUNCIONAR
await Api.Services.DockerMySqlManager.EnsureMySqlContainerRunningAsync(connString);
//ESPERA MY SQL ACORDAR PARA APLICAR AS MIGRATIONS
await IdentityCampaign.Infrastructure.MigrationHelper.WaitForMySqlAsync(connString);
//APLICA AS MIGRATIONS
IdentityCampaign.Infrastructure.MigrationHelper.ApplyMigrations(app);
#endregion

app.Run();
