using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infotecs.Internship.Application.Contracts;
using Infotecs.Internship.Application.Handlers.GetResultsQuery;
using Infotecs.Internship.Application.Handlers.GetValuesQuery;
using Infotecs.Internship.Application.Options;
using Infotecs.Internship.Application.Pagination;
using Infotecs.Internship.Domain.Entities;
using Infotecs.Internship.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infotecs.Internship.Infrastructure.Services;

public class FileRepository : IFileRepository
{
    private readonly OperationsFileDbContext _context;
    private readonly ValidationOptions _options;
    private readonly IMapper _mapper;

    public FileRepository(OperationsFileDbContext context, IMapper mapper, IOptionsSnapshot<ValidationOptions> options)
    {
        _context = context;
        _mapper = mapper;
        _options = options.Value;
    }

    public async Task<PaginatedList<OperationValuesWithFileName>> GetPagedValuesByQueryAsync(
        GetValuesQuery searchQuery,
        CancellationToken cancellationToken)
    {
        var baseQuery = _context.OperationValues
            .Include(v => v.ParentFile)
            .Where(v => EF.Functions.ILike(v.ParentFile.Name, searchQuery.FileName + "%"));

        // Группировка по файлам
        var groupedQuery = baseQuery
            .GroupBy(v => new { v.ParentFileId, v.ParentFile.Name })
            .Select(g => new OperationValuesWithFileName
            {
                FileName = g.Key.Name,
                Values = g
                    .OrderByDescending(v => v.StartedAt)
                    .Take(10)
                    .ToList()
            });
        
        groupedQuery = searchQuery.OrderByAscending
            ? groupedQuery.OrderBy(x => x.FileName)
            : groupedQuery.OrderByDescending(x => x.FileName);

        var paged = await groupedQuery.ToPaginatedListAsync(
            searchQuery.PageNumber,
            searchQuery.PageSize,
            _options.MinPageSize,
            _options.MaxPageSize,
            cancellationToken);

        return paged;
    }

    public async Task<PaginatedList<OperationsResultWithFileName>> GetPagedResultsByQueryAsync(GetResultsQuery searchQuery, CancellationToken cancellationToken)
    {
        var query = _context.OperationsResults.AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchQuery.FileName))
        {
            query = query.Where(x => EF.Functions.ILike(x.ParentFile.Name, searchQuery.FileName + "%"));
        }

        if (searchQuery.FromStartDate.HasValue)
        {
            query = query.Where(x => x.EarliestStartDate >= searchQuery.FromStartDate);
        }
        
        if (searchQuery.ToStartDate.HasValue)
        {
            query = query.Where(x => x.EarliestStartDate <= searchQuery.ToStartDate);
        }

        if (searchQuery.FromAverageValue.HasValue)
        {
            query = query.Where(x => x.AverageValue >= searchQuery.FromAverageValue);
        }

        if (searchQuery.ToAverageValue.HasValue)
        {
            query = query.Where(x => x.AverageValue <= searchQuery.ToAverageValue);
        }

        if (searchQuery.FromAverageDurationTimeSeconds.HasValue)
        {
            query = query.Where(x => x.AverageDurationTimeSeconds >= searchQuery.FromAverageDurationTimeSeconds);
        }

        if (searchQuery.ToAverageDurationTimeSeconds.HasValue)
        {
            query = query.Where(x => x.AverageDurationTimeSeconds <= searchQuery.ToAverageDurationTimeSeconds);
        }

        if (!searchQuery.OrderByAscending)
        {
            query = query.OrderByDescending(x => x.EarliestStartDate);
        }
        else
        {
            query = query.OrderBy(x => x.EarliestStartDate);
        }
        
        return await query
            .ProjectTo<OperationsResultWithFileName>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(searchQuery.PageNumber, searchQuery.PageSize, _options.MinPageSize, _options.MaxPageSize, cancellationToken);
    }
    
    public async Task<List<long>> GetIdsByNameAsync(string fileName, CancellationToken cancellationToken)
    {
        var entityIds = await _context.OperationsFiles
            .Where(f => EF.Functions.ILike(f.Name, fileName))
            .Select(f => f.Id)
            .ToListAsync(cancellationToken);
        return entityIds;
    }

    public async Task CascadeDeleteByIdsAsync(List<long> fileId, CancellationToken cancellationToken)
    {
        await _context.OperationsFiles
            .Where(f => fileId.Contains(f.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task AddFileAsync(OperationsFile fileEntity, CancellationToken cancellationToken = default)
    {
        await _context.OperationsFiles.AddAsync(fileEntity, cancellationToken);
    }
}