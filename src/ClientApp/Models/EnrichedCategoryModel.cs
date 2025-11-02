namespace ClientApp.Models;

public class EnrichedCategoryModel
{
    public string Html { get; set; } = "No rich text available.";
    
    public required string Id { get; set; }
    
    public required string Name { get; set; }
}