using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace ContentApp.Migrations;

public class DemoMigrationr(IRecipeMigrator recipes) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await recipes.ExecuteAsync("demo.json", this);
        return 1;
    }
}