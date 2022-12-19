using System.ComponentModel.DataAnnotations;

namespace server.Models.Filters;

public abstract class FilterParameters
{
    [Required]
    public int CurrentPage { get; set; } = 1;
    public string? ColumnToSort { get; set; }
    public string SortType { get; set; } = "asc";
}