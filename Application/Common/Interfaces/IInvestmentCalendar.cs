using Investment.Domain.Dto;

namespace Investment.Application.Common.Interfaces;

public interface IInvestmentCalendar
{
    InvestmentDays GetInvestmentDays(InvestmentDto investmentDto);
}

public record InvestmentDays
{
    public decimal RegularYearDays { get; set; }
    public decimal LeapYearDays { get; set; }
}