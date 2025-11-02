using OrchardCore.Data.Migration;

namespace ContentApp.Migrations;

public static class MigrationBootstrapper
{
    public static void AddMigrations(this IServiceCollection services)
    {
        services.AddDataMigration<DemoMigrationr>();
    }
}