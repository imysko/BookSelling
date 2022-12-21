namespace server.Models.Filters;

public class SalesFilter : FilterParameters
{
    public string? SellerName { get; set; }
    public string? SellerSurname { get; set; }

    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
}