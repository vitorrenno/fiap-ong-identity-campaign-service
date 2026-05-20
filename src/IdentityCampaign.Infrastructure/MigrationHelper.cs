using IdentityCampaign.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace IdentityCampaign.Infrastructure;

public static class MigrationHelper
{
    public static void ApplyMigrations(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IdentityCampaignDbContext>();
        
        try
        {
            db.Database.Migrate();
            Console.WriteLine("Migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error applying migrations: {ex.Message}");
            throw;
        }
    }
    public static async Task WaitForMySqlAsync(string connectionString)
    {
        var maxRetries = 30;
        var delaySeconds = 5;

        string[] strConn = connectionString.Split(";");
        for (int i = 1; i <= maxRetries; i++)
        {
            try
            {
                //RETIRANDO O NOME DO DATABASE POIS ELE PODE NÃO EXISTIR
                connectionString = $"{strConn[0]};{strConn[1]};{strConn[3]};{strConn[4]};";
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] MySQL está pronto! Conexão estabelecida com sucesso.");
                return;
            }
            catch (Exception ex)
            {
                if (i == maxRetries)
                {
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERRO: MySQL não ficou pronto após {maxRetries} tentativas.");
                    throw;
                }
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Tentativa {i}/{maxRetries} falhou: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            }
        }

        throw new Exception("MySQL não ficou pronto dentro do tempo esperado.");
    }

}
