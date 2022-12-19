namespace server.Models.PaginatedList;

public class PaginatedList<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    
    public PaginatedList(IEnumerable<T> items, int totalCount, int currentPage, int pageSize)
    {
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        PageSize = pageSize;
        HasPreviousPage = CurrentPage > 1;
        HasNextPage = CurrentPage < TotalPages;
        Items = items;
        TotalCount = totalCount;
    }
    
    public static Task<PaginatedList<T>> Create(IEnumerable<T> source, int currentPage, int pageSize)
    {
        var totalCount = source.Count();
        var items = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        return Task.FromResult(new PaginatedList<T>(items, totalCount, currentPage, pageSize));
    }
}