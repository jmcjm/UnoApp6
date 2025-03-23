using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UnoLib1;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
}

public class DatabaseInitializer(UnoLibDbContext context, ILogger<DatabaseInitializer> logger) : IDatabaseInitializer
{
    public async Task InitializeAsync()
    {
        try
        {
            logger.LogInformation("Applying migrations...");
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }
}
