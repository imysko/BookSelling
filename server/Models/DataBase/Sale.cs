namespace server.Models.DataBase;

public partial class Sale
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public int SellerId { get; set; }

    public int SoldCount { get; set; }

    public int Date { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Seller Seller { get; set; } = null!;
}
