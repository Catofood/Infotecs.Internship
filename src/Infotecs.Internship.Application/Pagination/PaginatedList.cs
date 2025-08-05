namespace Infotecs.Internship.Application.Pagination;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int PageCount => PageSize > 0 ? (int)Math.Ceiling((double)Items.Count / PageSize) : 0;
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < PageCount;

    public PaginatedList(IEnumerable<T> items, int pageNumber, int pageSize)
    {
        Items = items.ToList();
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

