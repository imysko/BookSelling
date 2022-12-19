using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context;
using server.Models.DataBase;
using server.Models.Filters;
using server.Models.PaginatedList;

namespace server.Controllers;

[Route("api/books")]
[ApiController]
[Produces("application/json")]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly BookSellingContext _context;
    private readonly IConfiguration _configuration;

    public BooksController(BookSellingContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult<PaginatedList<Book>>> GetBooks([FromQuery] BooksFilter filter)
    {
        IEnumerable<Book> books = await _context.Books
            .Include(b => b.BooksGenres)
            .ThenInclude(e => e.Genre)
            .ToListAsync();
        
        decimal yearMin = 0;
        decimal yearMax = 0;
        var priceMin = 0;
        var priceMax = 0;
        
        if (books.Any())
        {
            yearMin = (decimal)books.Min(i => i.Year);
            yearMax = (decimal)books.Max(i => i.Year);
            priceMin = (int)books.Min(i => i.Price);
            priceMax = (int)books.Max(i => i.Price);
        }

        books = filter.ColumnToSort switch
        {
            "name" => filter.SortType == "desc" ? books.OrderByDescending(b => b.Name) : books.OrderBy(b => b.Name),
            "author" => filter.SortType == "desc" ? books.OrderByDescending(b => b.Author) : books.OrderBy(b => b.Author),
            "year" => filter.SortType == "desc" ? books.OrderByDescending(b => b.Year) : books.OrderBy(b => b.Year),
            "price" => filter.SortType == "desc" ? books.OrderByDescending(b => b.Price) : books.OrderBy(b => b.Price),
            _ => books
        };

        books = books
            .Where(b => filter.IncludeNotActive || b.Active)
            .Where(b => filter.Name == null || b.Name.ToLower().Contains(filter.Name.ToLower()))
            .Where(b => filter.Author == null || b.Author!.ToLower().Contains(filter.Author.ToLower()))
            .Where(b => b.Year >= filter.YearMin && b.Year <= filter.YearMax)
            .Where(b => b.Price >= filter.PriceMin && b.Price <= filter.PriceMax)
            .Where(b => filter.Genries == null || b.BooksGenres
                            .Select(bg => bg.Genre.Name)
                            .Any(genre => filter.Genries.Contains(genre)));
        
        var pageSize = _configuration.GetValue("PagesSize:BookPage", 10);
        var list = await BooksPaginatedList<Book>.GetPage(books, filter.CurrentPage, pageSize);

        list.YearMin = yearMin;
        list.YearMax = yearMax;
        list.PriceMin = priceMin;
        list.PriceMax = priceMax;

        return list;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetGenre(int id)
    {
        var book = await _context.Books
            .Include(b => b.BooksGenres)
            .ThenInclude(e => e.Genre)
            .FirstOrDefaultAsync(b => b.Id == id);

        return book == null ? NotFound() : book;
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPost, Authorize(Roles = "editor, superuser")]
    public async Task<IActionResult> PostBook(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return StatusCode(201, "Book was created");
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPut("{id}/buy"), Authorize(Roles = "user")]
    public async Task<IActionResult> BuyBook(int id, [Required]int count)
    {
        if (!BookExists(id))
        {
            return NotFound();
        }
        
        var book = await _context.Books.FindAsync(id);

        if (count > book!.StoreCount)
        {
            return BadRequest();
        }

        book.StoreCount -= count;

        if (book.StoreCount == 0)
        {
            book.Active = false;
        }
        
        _context.Entry(book).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return Ok("Book was bought");
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPut("{id}"), Authorize(Roles = "editor, superuser")]
    public async Task<IActionResult> PutBook(int id, Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        _context.Entry(book).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return Ok("Book was changed");
    }
    
        
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPut("{id}/archived"), Authorize(Roles = "editor, superuser")]  
    public async Task<IActionResult> ArchivedBook(int id)
    {
        if (!BookExists(id))
        {
            return NotFound();
        }
        
        var book = await _context.Books.FindAsync(id);

        book!.Active = false;
        
        _context.Entry(book).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return Ok("Book was bought");
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpDelete("{id}"), Authorize(Roles = "editor, superuser")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return Ok("Book was deleted");
    }

    private bool BookExists(int id)
    {
        return (_context.Books?.Any(b => b.Id == id)).GetValueOrDefault();
    }
}