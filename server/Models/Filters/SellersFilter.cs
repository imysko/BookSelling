namespace server.Models.Filters;

public class SellersFilter : FilterParameters
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Fname { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IncludeNotActive { get; set; } = false;
}