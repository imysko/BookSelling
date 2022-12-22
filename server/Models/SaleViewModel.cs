using server.Models.DataBase;

namespace server.Models;

public class SaleViewModel
{
    public Sale Sale { get; set; }
    public int BookId { get; set; }
    public int SoldCount { get; set; }
}