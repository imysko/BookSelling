using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    [HttpGet, AllowAnonymous]
    public async Task<ActionResult<PaginatedList<Seller>>> GetSellers([FromQuery] SellersFilter filter)
    {
        IEnumerable<Seller> sellers = await _context.Sellers.ToListAsync();

        sellers = filter.ColumnToSort switch
        {
            "name" => filter.SortType == "desc" ? sellers.OrderByDescending(b => b.Name) : sellers.OrderBy(b => b.Name),
            "surname" => filter.SortType == "desc" ? sellers.OrderByDescending(b => b.Surname) : sellers.OrderBy(b => b.Surname),
            "fname" => filter.SortType == "desc" ? sellers.OrderByDescending(b => b.Fname) : sellers.OrderBy(b => b.Fname),
            "phone_number" => filter.SortType == "desc" ? sellers.OrderByDescending(b => b.PhoneNumber) : sellers.OrderBy(b => b.PhoneNumber),
            _ => sellers
        };

        sellers = sellers
            .Where(b => filter.IncludeNotActive || b.Active)
            .Where(b => filter.Name == null || b.Name.ToLower().Contains(filter.Name.ToLower()))
            .Where(b => filter.Surname == null || b.Surname.ToLower().Contains(filter.Surname.ToLower()))
            .Where(b => filter.Fname == null || b.Fname!.ToLower().Contains(filter.Fname.ToLower()))
            .Where(b => filter.PhoneNumber == null ||
                        b.PhoneNumber.ToString()!.ToLower().Contains(filter.PhoneNumber.ToLower()));

        var pageSize = _configuration.GetValue("PagesSize:SellerPage", 10);
        return await PaginatedList<Seller>.Create(sellers, filter.CurrentPage, pageSize);
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpGet("{id}"), AllowAnonymous]
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
        return (_context.Sellers?.Any(g => g.Id == id)).GetValueOrDefault();
    }
}