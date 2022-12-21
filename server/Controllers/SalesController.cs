using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context;
using server.Models.DataBase;
using server.Models.Filters;
using server.Models.PaginatedList;

namespace server.Controllers;

[Route("api/sales")]
[ApiController]
[Produces("application/json")]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly BookSellingContext _context;
    private readonly IConfiguration _configuration;
    
    public SalesController(BookSellingContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }   
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet, Authorize(Roles = "seller, admin, superuser")]
    public async Task<ActionResult<PaginatedList<Sale>>> GetSales([FromQuery] SalesFilter filter)
    {
        IEnumerable<Sale> sales = await _context.Sales
            .IgnoreAutoIncludes()
            .Include(s => s.Seller)
            .Include(s => s.SalesBooks)
            .ThenInclude(sb => sb.Book)
            .ToListAsync();
        
        var dateMin = DateTime.Now.Date;
        var dateMax = DateTime.Now.Date;

        if (sales.Any())
        {
            dateMin = sales.Min(s => s.Date);
            dateMax = sales.Max(s => s.Date);
        }

        sales = filter.ColumnToSort switch
        {
            "seller_name" => filter.SortType == "desc" ? sales.OrderByDescending(s => s.Seller.Name)
                : sales.OrderBy(s => s.Seller.Name),
            "date" => filter.SortType == "desc" ? sales.OrderByDescending(s => s.Date)
                : sales.OrderBy(s => s.Date),
            _ => sales
        };

        sales = sales
            .Where(s => filter.SellerName == null || s.Seller.Name.ToLower().Contains(filter.SellerName.ToLower()))
            .Where(s => filter.SellerSurname == null || s.Seller.Surname.ToLower().Contains(filter.SellerSurname.ToLower()))
            .Where(s => filter.MinDate == null || s.Date >= filter.MinDate)
            .Where(s => filter.MaxDate == null || s.Date <= filter.MaxDate);

        var pageSize = _configuration.GetValue("PagesSize:SalesPage", 10);
        var list = await SalesPaginatedList<Sale>.GetPage(sales, filter.CurrentPage, pageSize);

        list.DateMin = dateMin;
        list.DateMax = dateMax;

        return list;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("{id}"), Authorize(Roles = "seller, admin, superuser")]
    public async Task<ActionResult<Sale>> GetSale(int id)
    {
        var sales = await _context.Sales
            .Include(s => s.Seller)
            .Include(s => s.SalesBooks)
            .ThenInclude(sb => sb.Book)
            .FirstOrDefaultAsync(s => s.Id == id);

        return sales == null ? NotFound() : sales;
    }
}