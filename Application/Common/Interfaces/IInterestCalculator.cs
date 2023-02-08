using Investment.Domain.Dto;

namespace Investment.Application.Common.Interfaces;

public interface IInterestCalculator
{
    decimal CalculateInterest(InvestmentDto investmentDto);
}