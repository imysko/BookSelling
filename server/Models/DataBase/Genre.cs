﻿namespace server.Models.DataBase;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    public virtual ICollection<BookGenre> BooksGenres { get; set; }
}
