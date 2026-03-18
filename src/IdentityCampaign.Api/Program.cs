using IdentityCampaign.Application.Abstractions;
using IdentityCampaign.Infrastructure.Persistence;
using IdentityCampaign.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using IdentityCampaign.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<IdentityCampaignDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(IdentityCampaign.Application.Features.Campaigns.CreateCampaign.CreateCampaignHandler).Assembly));

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