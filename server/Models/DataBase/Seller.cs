namespace server.Models.DataBase;

public partial class Seller
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Fname { get; set; }

    public decimal? FhoneNumber { get; set; }

    public virtual ICollection<Sale> Sales { get; } = new List<Sale>();
}
