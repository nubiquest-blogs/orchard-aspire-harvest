using DemoContracts;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Models;
using OrchardCore.Markdown.Models;

namespace ContentApp.Controllers;

[ApiController]
public class ContentController(
    IContentManager content,
    IContentHandleManager handle,
    ILogger<ContentController> logger) : ControllerBase
{
    [HttpGet("/content/categories/{id}")]
    public async Task<IActionResult?> GetApiCategory(string id)
    {
        var body = await GetBody(id);
        if (body == null)
        {
            return NotFound();
        }

        return Ok(new CategoryContentResponse() { Id = id, Html = body });
    }


    private async Task<string?> GetContentItemBy(string method, string contentId)
    {
        logger.LogInformation("Getting content item by method: {Method}, contentId: {ContentId}", method, contentId);
        var id = await handle.GetContentItemIdAsync($"{method}:{contentId}");

        var result = await content.GetAsync(id, VersionOptions.Published);
        var bodyAspect = await content.PopulateAspectAsync<BodyAspect>(result);
        return bodyAspect?.Body.ToString();
    }

    private Task<string?> GetBody(string alias) => GetContentItemBy("alias", alias);
}