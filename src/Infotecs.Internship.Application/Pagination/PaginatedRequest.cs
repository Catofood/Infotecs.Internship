using System.ComponentModel;

namespace Infotecs.Internship.Application.Pagination;

public abstract record PaginatedRequest
{
    [DefaultValue(1)]
    public int PageNumber { get; init; }
    [DefaultValue(10)]
    public int PageSize { get; init; }
    [DefaultValue(false)]
    public bool OrderByAscending { get; init; }
}