using ClientApp.Models;

namespace ClientApp.Services;

public interface ICategoryService
{
    Task<EnrichedCategoryModel> GetCategory(string id);
}