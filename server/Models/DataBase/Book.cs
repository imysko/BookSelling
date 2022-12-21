namespace server.Models.DataBase;

public partial class Book
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Author { get; set; }

    public decimal? Year { get; set; }

    public int? Price { get; set; }

    public int StoreCount { get; set; }
    
    public string Image { get; set; } = null!;

    public bool Active { get; set; } = true;
    
    public virtual ICollection<BookGenre> BooksGenres { get; set; }

    public virtual ICollection<Sale> Sales { get; } = new List<Sale>();
}
