using Investment.Domain.Dto;

namespace Investment.Application.Common.Interfaces;

public interface IInvestmentCalculator
{
    decimal CalculateInterest(InvestmentDto investmentDto);
}