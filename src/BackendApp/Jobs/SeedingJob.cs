using BackendApp.Database;

namespace BackendApp.Jobs;

public class SeedingJob(IServiceScopeFactory services, ILogger<SeedingJob> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = services.CreateScope();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            logger.LogInformation("Seeding database, {ConnectionString}", config.GetConnectionString("postgresdb"));
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.EnsureCreatedAsync(stoppingToken);
            await CreateCategory(db, "ai", "Artificial Intelligence", stoppingToken);
            await CreateCategory(db, "cl", "Cloud Services", stoppingToken);
            await CreateCategory(db, "devops", "DevOps", stoppingToken);
            await db.SaveChangesAsync(stoppingToken);
            logger.LogInformation("Database seeding completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to seed database");
        }
    }

    private async Task CreateCategory(AppDbContext db, string id, string name, CancellationToken stoppingToken)
    {
        var category = new CategoryEntity() { Id = id, Name = name };
        await db.Categories.AddAsync(category, stoppingToken);
    }
}