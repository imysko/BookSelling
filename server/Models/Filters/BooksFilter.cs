namespace server.Models.Filters;

public class BooksFilter : FilterParameters
{
    public string? Name { get; set; }
    public string? Author { get; set; }
    public bool IncludeNotActive { get; set; } = false;
    public int YearMin { get; set; } = 0;
    public int YearMax { get; set; } = int.MaxValue;
    public int PriceMin { get; set; } = 0;
    public int PriceMax { get; set; } = int.MaxValue;
    public List<string>? Genries { get; set; }
}