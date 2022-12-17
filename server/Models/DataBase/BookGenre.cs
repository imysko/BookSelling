using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models.DataBase;

public partial class BookGenre
{
    [Column("book_id")]
    public int BookId { get; set; }
    public Book Book { get; set; }
    
    [Column("genre_id")]
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
}