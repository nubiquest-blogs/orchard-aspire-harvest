using System.Text.Json;
using DemoContracts;

namespace ClientApp.Clients;

public class ContentClient(HttpClient client)
{
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public async Task<CategoryContentResponse?> GetCategoryContent(string id)
    {
        var response = await client.GetAsync($"/content/categories/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<CategoryContentResponse>(_options);
    }
}