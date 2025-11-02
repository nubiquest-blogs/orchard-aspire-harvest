using ClientApp.Clients;
using ClientApp.Models;

namespace ClientApp.Services;

public class CategoryService(BackendClient backendClient, ContentClient contentClient) : ICategoryService
{
    public async Task<EnrichedCategoryModel> GetCategory(string id)
    {
        var category = await backendClient.GetCategory(id);

        var result = new EnrichedCategoryModel
        {
            Id = category.Id,
            Name = category.Name,
        };

        var content = await contentClient.GetCategoryContent(id);
        if (content != null)
        {
            result.Html = content.Html;
        }

        return result;
    }
}