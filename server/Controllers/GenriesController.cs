using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context;
using server.Models.DataBase;

namespace server.Controllers;

[Route("api/genries")]
[ApiController]
[Produces("application/json")]
[Authorize]
public class GenriesController : ControllerBase
{
    private readonly BookSellingContext _context;

    public GenriesController(BookSellingContext context)
    {
        _context = context;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Genre>>> GetGenries()
    {
        return await _context.Genries
            .OrderBy(g => g.Name)
            .ToListAsync();
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Genre>> GetGenre(int id)
    {
        var genre = await _context.Genries.FindAsync(id);

        return genre == null ? NotFound() : genre;
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPost, Authorize(Roles = "editor, superuser")]
    public async Task<IActionResult> PostGenre(Genre genre)
    {
        _context.Genries.Add(genre);
        await _context.SaveChangesAsync();

        return StatusCode(201, "Genre was created");
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPut("{id}"), Authorize(Roles = "editor, superuser")]
    public async Task<IActionResult> PutGenre(int id, Genre genre)
    {
        if (id != genre.Id)
        {
            return BadRequest();
        }

        _context.Entry(genre).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GenreExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return Ok("Genre was changed");
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpDelete("{id}"), Authorize(Roles = "editor, superuser")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        var genre = await _context.Genries.FindAsync(id);
        if (genre == null)
        {
            return NotFound();
        }

        _context.Genries.Remove(genre);
        await _context.SaveChangesAsync();

        return Ok("Genre was deleted");
    }

    private bool GenreExists(int id)
    {
        return (_context.Genries?.Any(g => g.Id == id)).GetValueOrDefault();
    }
}