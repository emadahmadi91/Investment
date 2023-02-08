using Investment.Application.Common.Interfaces;
using Investment.Domain.Dto;

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
}