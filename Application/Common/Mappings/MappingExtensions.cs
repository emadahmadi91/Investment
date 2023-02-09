using Investment.Application.Common.Interfaces;
using Investment.Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace Investment.Application.Common.Mappings;

public static class MappingExtensions
{
    public static async Task<List<InvestmentDto>> CalculateInvestment(this Task<List<InvestmentDto>> list,
        IInterestCalculator interestCalculator)
    {
        var x = await list;
        x.ForEach(v => v.Value = interestCalculator.CalculateInterest(v));

        return await list;
    }
    
    public static  IEnumerable<InvestmentDto> CalculateInvestment(this IQueryable<InvestmentDto> list,
        IInterestCalculator interestCalculator)
    {
        return list.AsEnumerable().Select(dto =>
        {
            dto.Value = interestCalculator.CalculateInterest(dto);
            return dto;
        });
    }
}