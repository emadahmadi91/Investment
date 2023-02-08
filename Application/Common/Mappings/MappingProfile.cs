using AutoMapper;
using Investment.Domain.Dto;

namespace Investment.Application.Common.Mappings;

using Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DateTime, string>().ConvertUsing(dt => dt.ToString("yyyy-dd-MM"));
        CreateMap<InvestmentDto, Investment>();
        CreateMap<Investment, InvestmentDto>();
    }
}
