namespace server.Models.DataBase;

public partial class Sale
{
    public int Id { get; set; }

    public int? SellerId { get; set; }

    public DateTime Date { get; set; }

    public virtual Seller? Seller { get; set; }
    
    public virtual ICollection<SaleBook>? SalesBooks { get; set; }
}
