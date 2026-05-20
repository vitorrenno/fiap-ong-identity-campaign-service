using IdentityCampaign.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace IdentityCampaign.Infrastructure;

public static class MigrationHelper
{
    private static readonly object MigrationLock = new object();

    public static void ApplyMigrations(IApplicationBuilder app)
    {
        // Use lock to prevent multiple pods from applying migrations simultaneously
        lock (MigrationLock)
        {
            var maxRetries = 5;
            var retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    using var scope = app.ApplicationServices.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<IdentityCampaignDbContext>();

                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Attempting to apply migrations (Attempt {retryCount + 1}/{maxRetries})...");

                    // Ensure database exists
                    db.Database.EnsureCreated();
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Database ensured to exist.");

                    // Apply pending migrations
                    db.Database.Migrate();
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Migrations applied successfully.");
                    return;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Error on attempt {retryCount}: {ex.Message}");
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Stack trace: {ex.StackTrace}");

                    if (retryCount >= maxRetries)
                    {
                        Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERRO CRÍTICO: Não foi possível aplicar migrations após {maxRetries} tentativas.");
                        throw;
                    }

                    // Exponential backoff: 2s, 4s, 8s, 16s, 32s
                    var delayMs = (int)Math.Pow(2, retryCount) * 1000;
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Aguardando {delayMs}ms antes da próxima tentativa...");
                    System.Threading.Thread.Sleep(delayMs);
                }
            }
        }
    }

    public static async Task WaitForMySqlAsync(string connectionString)
    {
        var maxRetries = 60;  // Increased from 30 to 60 for Kubernetes environments
        var delaySeconds = 2;
        var originalConnectionString = connectionString;

        for (int i = 1; i <= maxRetries; i++)
        {
            try
            {
                // Extract connection parts without the database name
                string[] strConn = originalConnectionString.Split(";");
                var connectionWithoutDb = ExtractConnectionStringWithoutDatabase(strConn);

                using var connection = new MySqlConnection(connectionWithoutDb);
                await connection.OpenAsync();
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ✓ MySQL server está pronto! Conexão estabelecida com sucesso.");

                // Now verify that we can connect with the full connection string (including database)
                await VerifyDatabaseConnectionAsync(originalConnectionString, i, maxRetries);
                return;
            }
            catch (Exception ex)
            {
                if (i == maxRetries)
                {
                    Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ✗ ERRO: MySQL não ficou pronto após {maxRetries} tentativas.");
                    throw new Exception($"MySQL connection failed after {maxRetries} attempts: {ex.Message}", ex);
                }

                var delayMs = delaySeconds * 1000;
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Tentativa {i}/{maxRetries} falhou. Aguardando {delaySeconds}s... Erro: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            }
        }

        throw new Exception("MySQL não ficou pronto dentro do tempo esperado.");
    }

    private static async Task VerifyDatabaseConnectionAsync(string fullConnectionString, int attempt, int maxRetries)
    {
        try
        {
            using var connection = new MySqlConnection(fullConnectionString);
            await connection.OpenAsync();
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ✓ Database connection verified successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ⚠ Database connection verification failed (may not exist yet, will be created by migrations): {ex.Message}");
            // Don't throw - the database might not exist yet, which is fine
        }
    }

    private static string ExtractConnectionStringWithoutDatabase(string[] connectionParts)
    {
        try
        {
            // Build connection string without database name
            var result = string.Empty;
            foreach (var part in connectionParts)
            {
                if (!part.StartsWith("Database=", StringComparison.OrdinalIgnoreCase) &&
                    !part.StartsWith("Initial Catalog=", StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrWhiteSpace(part))
                {
                    result += part + ";";
                }
            }
            return result;
        }
        catch
        {
            // Fallback: return first few parts (Server, UserId, Password)
            return $"{connectionParts[0]};{connectionParts[1]};{connectionParts[2]};";
        }
    }
}
