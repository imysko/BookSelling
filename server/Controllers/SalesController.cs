using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Context;
using server.Models;
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
            .Include(s => s.SalesBooks)!
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
            .Include(s => s.SalesBooks)!
            .ThenInclude(sb => sb.Book)
            .FirstOrDefaultAsync(s => s.Id == id);

        return sales == null ? NotFound() : sales;
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpPost, Authorize(Roles = "user, seller, superuser")]
    public async Task<IActionResult> PostSale(SaleViewModel viewModel)
    {
        var book = await _context.Books.FindAsync(viewModel.BookId);
        
        if (book == null)
        {
            return NotFound("Book not found");
        }

        var saleBook = new SaleBook
        {
            Sale = viewModel.Sale,
            BookId = viewModel.BookId,
            SoldCount = viewModel.SoldCount
        };

        viewModel.Sale.SalesBooks = new List<SaleBook>();
        viewModel.Sale.SalesBooks.Add(saleBook);
        
        _context.Sales.Add(viewModel.Sale);
        await _context.SaveChangesAsync();

        return StatusCode(201, "Sale was created");
    }
    
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPut("{id}"), Authorize(Roles = "admin, superuser")]
    public async Task<IActionResult> ConfirmSale(int id, int sellerId)
    {
        var sale = await _context.Sales.FindAsync(id);
        if (sale == null)
        {
            return NotFound("Sale not found");
        }
        if (sale.SellerId != null)
        {
            return Conflict("Sale already confirmed");
        }
        
        var seller = await _context.Sellers.FindAsync(sellerId);
        if (seller == null)
        {
            return NotFound("Seller not found");
        }
        
        sale.SellerId = sellerId;
        
        _context.Entry(sale).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return StatusCode(201, "Sale was confirmed");
    }
}