using Infotecs.Internship.Application.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Internship.Infrastructure.Extensions;

public static class PaginationExtensions
{
    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        int minPageSize,
        int maxPageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);

        if (count == 0)
        {
            return new PaginatedList<T>(new List<T>(), 1, pageSize);
        }
        
        var correctedPageSize = pageSize < minPageSize ? minPageSize
            : pageSize > maxPageSize ? maxPageSize
            : pageSize;

        var totalPages = (int)Math.Ceiling(count / (double)correctedPageSize);
        
        var correctedPageNumber = pageNumber < 1 ? 1 : pageNumber > totalPages ? totalPages : pageNumber;

        var items = await source
            .Skip((correctedPageNumber - 1) * correctedPageSize)
            .Take(correctedPageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, correctedPageNumber, correctedPageSize);
    }
}

