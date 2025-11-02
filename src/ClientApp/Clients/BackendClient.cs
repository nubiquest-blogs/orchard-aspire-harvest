using System.Text.Json;
using DemoContracts;

namespace ClientApp.Clients;

public class BackendClient(HttpClient client)
{
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public async Task<CategoryResponse> GetCategory(string id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/categories/{id}");
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Call failed");
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonSerializer.Deserialize<CategoryResponse>(content, _options);

        return result!;
    }
}