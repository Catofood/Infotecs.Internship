using AutoMapper;
using Infotecs.Internship.Api.Dto;
using Infotecs.Internship.Application.Handlers.GetResultsQuery;
using Infotecs.Internship.Application.Handlers.GetValuesQuery;
using Infotecs.Internship.Domain.Entities;


namespace Infotecs.Internship.Api.Mapping;

public class ApplicationToApiProfile : Profile
{
    public ApplicationToApiProfile()
    {
        CreateMap<OperationsResultWithFileName, OperationsResultWithFileNameDto>();
        CreateMap<OperationValuesWithFileName, OperationValuesWithFileNameDto>() // В OperationValuesWithFileName имя ещё есть
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(x => x.FileName))
            .ForMember(dest => dest.PaginatedValues, opt => opt.MapFrom(x => x.Values));
        CreateMap<OperationValue, OperationValueDto>();
    }
}