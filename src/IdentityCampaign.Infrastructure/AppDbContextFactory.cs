using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using IdentityCampaign.Infrastructure.Persistence;
namespace IdentityCampaign.Infrastructure;

public class AppDbContextFactory: IDesignTimeDbContextFactory<IdentityCampaignDbContext>
{
    public IdentityCampaignDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityCampaignDbContext>();
            //Essa conectionString só será necessária para criar o banco pela primeira vez, depois disso, o banco já estará criado e a aplicação irá usar a conectionString do appsettings.json
            optionsBuilder.UseMySql
            (
                "Server=localhost;Port=3306;Database=Bd_Campaing;Uid=root;Pwd=1234;",
                new MySqlServerVersion(new Version(8, 0, 43))
            );
            return new IdentityCampaignDbContext(optionsBuilder.Options);
        }
    
}
