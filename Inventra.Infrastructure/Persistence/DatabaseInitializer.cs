using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inventra.Infrastructure.Persistence
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(DatabaseContext context, ILogger<DatabaseContext> logger)
        {
            try
            {
                // Ensure database is created
                await context.Database.EnsureCreatedAsync();

                // Apply pending migrations
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Applying {MigrationCount} pending migrations...", pendingMigrations.Count());
                    await context.Database.MigrateAsync();
                }

                logger.LogInformation("Database initialized successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }
    }
}