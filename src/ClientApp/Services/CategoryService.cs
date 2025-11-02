using ClientApp.Clients;
using ClientApp.Models;
using StackExchange.Redis;

namespace ClientApp.Services;

public class CategoryService : ICategoryService
{
    private readonly BackendClient _backendClient;
    private readonly ContentClient _contentClient;
    private readonly IDatabase _db;
    private ILogger<CategoryService> _logger;

    public CategoryService(BackendClient backendClient,
        ContentClient contentClient,
        IConnectionMultiplexer cache,
        ILogger<CategoryService> logger)
    {
        _backendClient = backendClient;
        _contentClient = contentClient;
        _db = cache.GetDatabase();
        _logger = logger;
    }

    public async Task<EnrichedCategoryModel> GetCategory(string id)
    {
        var cacheKey = $"category:{id}";
        var cached = await _db.StringGetAsync(cacheKey);
        if (cached.HasValue)
        {
            _logger.LogInformation("Category {Id} retrieved from cache", id);
            return System.Text.Json.JsonSerializer.Deserialize<EnrichedCategoryModel>(cached!)!;
        }

        var category = await _backendClient.GetCategory(id);

        var result = new EnrichedCategoryModel
        {
            Id = category.Id,
            Name = category.Name,
        };

        var content = await _contentClient.GetCategoryContent(id);

        if (content != null)
        {
            result.Html = content.Html;
            await _db.StringSetAsync(cacheKey, System.Text.Json.JsonSerializer.Serialize(result),
                TimeSpan.FromHours(1));
            _logger.LogInformation("Category {Id} retrieved from source", id);
        }


        return result;
    }
}