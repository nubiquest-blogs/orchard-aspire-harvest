using ClientApp.Clients;
using ClientApp.Models;

namespace ClientApp.Services;

public class CategoryService(BackendClient backendClient) : ICategoryService
{
    public async Task<EnrichedCategoryModel> GetCategory(string id)
    {
        var category = await backendClient.GetCategory(id);

        return new EnrichedCategoryModel()
        {
            Id = category.Id,
            Name = category.Name,
            Html = $"<h1>{category.Name}</h1><p>Category ID: {category.Id}</p>"
        };
    }
}