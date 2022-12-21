namespace server.Models.PaginatedList;

public class SalesPaginatedList<T> : PaginatedList<T>
{
    public DateTime DateMin { get; set; } = DateTime.Now.Date;
    public DateTime DateMax { get; set; } = DateTime.Now.Date;
    
    public SalesPaginatedList(IEnumerable<T> items, int totalCount, int currentPage, int pageSize)
        : base(items, totalCount, currentPage, pageSize)
    {
    }
    
    public static Task<SalesPaginatedList<T>> GetPage(IEnumerable<T> source, int currentPage, int pageSize)
    {
        var totalCount = source.Count();
        var items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        return Task.FromResult(
            new SalesPaginatedList<T>(items, totalCount, currentPage, pageSize));
    }
}