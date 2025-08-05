using AutoMapper;
using Infotecs.Internship.Application.Handlers.GetResultsQuery;
using Infotecs.Internship.Application.Handlers.GetValuesQuery;
using Infotecs.Internship.Application.Pagination;
using Infotecs.Internship.Domain.Entities;

namespace Infotecs.Internship.Application.Mapping;

public class DomainToApplicationProfile : Profile
{
    public DomainToApplicationProfile()
    {
        CreateMap<OperationsResult, OperationsResultWithFileName>()
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.ParentFile.Name));
        
        CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
            .ConvertUsing(typeof(PaginatedListConverter<,>));
    }
}