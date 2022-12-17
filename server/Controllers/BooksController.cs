using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context;
using server.Models.DataBase;

namespace server.Controllers;

[Route("api/books")]
[ApiController]
[Produces("application/json")]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly BookSellingContext _context;

    public BooksController(BookSellingContext context)
    {
        _context = context;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Book>>> GetGenries()
    {
        return await _context.Books
            .Include(b => b.BooksGenres)
            .ThenInclude(e => e.Genre)
            .OrderBy(b => b.Name)
            .ToListAsync();
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
    public async Task<IActionResult> PostGenre(Book book)
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
    [HttpPut("{id}"), Authorize(Roles = "editor, superuser")]
    public async Task<IActionResult> PutGenre(int id, Book book)
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
    [HttpDelete("{id}"), Authorize(Roles = "editor, superuser")]
    public async Task<IActionResult> DeleteGenre(int id)
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