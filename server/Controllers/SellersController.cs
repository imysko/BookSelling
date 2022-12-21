using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context;
using server.Models.DataBase;
using server.Models.Filters;
using server.Models.PaginatedList;

namespace server.Controllers;

[Route("api/sellers")]
[ApiController]
[Produces("application/json")]
[Authorize]
public class SellersController : ControllerBase
{
    private readonly BookSellingContext _context;
    private readonly IConfiguration _configuration;
    
    public SellersController(BookSellingContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet, Authorize(Roles = "admin, superuser")]
    public async Task<ActionResult<PaginatedList<Seller>>> GetSellers([FromQuery] SellersFilter filter)
    {
        IEnumerable<Seller> sellers = await _context.Sellers.ToListAsync();

        sellers = filter.ColumnToSort switch
        {
            "name" => filter.SortType == "desc" ? sellers.OrderByDescending(s => s.Name) : sellers.OrderBy(s => s.Name),
            "surname" => filter.SortType == "desc" ? sellers.OrderByDescending(s => s.Surname) : sellers.OrderBy(s => s.Surname),
            "fname" => filter.SortType == "desc" ? sellers.OrderByDescending(s => s.Fname) : sellers.OrderBy(s => s.Fname),
            "phone_number" => filter.SortType == "desc" ? sellers.OrderByDescending(s => s.PhoneNumber) : sellers.OrderBy(s => s.PhoneNumber),
            _ => sellers
        };

        sellers = sellers
            .Where(s => filter.IncludeNotActive || s.Active)
            .Where(s => filter.Name == null || s.Name.ToLower().Contains(filter.Name.ToLower()))
            .Where(s => filter.Surname == null || s.Surname.ToLower().Contains(filter.Surname.ToLower()))
            .Where(s => filter.Fname == null || s.Fname!.ToLower().Contains(filter.Fname.ToLower()))
            .Where(s => filter.PhoneNumber == null ||
                        s.PhoneNumber.ToString()!.ToLower().Contains(filter.PhoneNumber.ToLower()));

        var pageSize = _configuration.GetValue("PagesSize:SellersPage", 10);
        return await PaginatedList<Seller>.Create(sellers, filter.CurrentPage, pageSize);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("{id}"), Authorize(Roles = "admin, superuser")]
    public async Task<ActionResult<Seller>> GetSeller(int id)
    {
        var seller = await _context.Sellers.FindAsync(id);

        return seller == null ? NotFound() : seller;
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPost, Authorize(Roles = "admin, superuser")]
    public async Task<IActionResult> PostSeller(Seller seller)
    {
        _context.Sellers.Add(seller);
        await _context.SaveChangesAsync();

        return StatusCode(201, "Seller was created");
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPut("{id}"), Authorize(Roles = "admin, superuser")]
    public async Task<IActionResult> PutSeller(int id, Seller seller)
    {
        if (id != seller.Id)
        {
            return BadRequest();
        }

        _context.Entry(seller).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SellerExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return Ok("Seller was changed");
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPut("{id}/archived"), Authorize(Roles = "admin, superuser")]  
    public async Task<IActionResult> ArchivedSeller(int id, bool active)
    {
        if (!SellerExists(id))
        {
            return NotFound();
        }
        
        var seller = await _context.Sellers.FindAsync(id);

        seller!.Active = active;
        
        _context.Entry(seller).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return Ok("Seller was changed");
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpDelete("{id}"), Authorize(Roles = "superuser")]
    public async Task<IActionResult> DeleteSeller(int id)
    {
        var seller = await _context.Sellers.FindAsync(id);
        if (seller == null)
        {
            return NotFound();
        }

        _context.Sellers.Remove(seller);
        await _context.SaveChangesAsync();

        return Ok("Seller was deleted");
    }
    
    private bool SellerExists(int id)
    {
        return (_context.Sellers?.Any(s => s.Id == id)).GetValueOrDefault();
    }
}