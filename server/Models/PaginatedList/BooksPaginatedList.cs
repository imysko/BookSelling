namespace server.Models.PaginatedList;

public class BooksPaginatedList<T> : PaginatedList<T>
{
    public decimal YearMin { get; set; } = 0;
    public decimal YearMax { get; set; } = 0;
    public int PriceMin { get; set; } = 0;
    public int PriceMax { get; set; } = 0;
    
    public BooksPaginatedList(IEnumerable<T> items, int totalCount, int currentPage, int pageSize)
        : base(items, totalCount, currentPage, pageSize)
    {
    }
    
    public static Task<BooksPaginatedList<T>> GetPage(IEnumerable<T> source, int currentPage, int pageSize)
    {
        var totalCount = source.Count();
        var items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        return Task.FromResult(
            new BooksPaginatedList<T>(items, totalCount, currentPage, pageSize));
    }
}