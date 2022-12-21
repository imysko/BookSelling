using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models.DataBase;

public partial class SaleBook
{
    [Column("sale_id")]
    public int SaleId { get; set; }
    public Sale Sale { get; set; }
    
    [Column("book_id")]
    public int BookId { get; set; }
    public Book Book { get; set; }
    
    public int SoldCount { get; set; }
}