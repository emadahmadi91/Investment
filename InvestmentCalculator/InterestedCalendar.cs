using Investment.Application.Common.Interfaces;
using Investment.Domain.Dto;

namespace Investment.InvestmentCalculator;

public class InvestmentCalendar : IInvestmentCalendar
{
    private const int LastMonthOfYear = 12;
    
    private const int LastDayOfYear = 31;
    
    private const int FirstMonthOfYear = 1;
    
    private const int FirstDayOfYear = 1;
    
    private const int DayCarriedForwardBetweenYear = 1;
    
    private readonly IDateTime _dateTime;

    public InvestmentCalendar(IDateTime dateTime)
    {
        _dateTime = dateTime;
    }

    public InvestmentDays GetInvestmentDays(InvestmentDto investmentDto)
    {
        var startDate = DateTime.Parse(investmentDto.StartDate);
        
        var investmentDays = new InvestmentDays() { LeapYearDays = 0, RegularYearDays = 0 };

        if (startDate > _dateTime.Now)
            return investmentDays;
        
        return CalculateDaysYearly(investmentDays, startDate);
    }

    private InvestmentDays CalculateDaysYearly(InvestmentDays investmentDays, DateTime lastCheckedDate)
    {
        var lastCheckedDateEndOfYear = new DateTime(lastCheckedDate.Year, LastMonthOfYear, LastDayOfYear);
        
        if (lastCheckedDateEndOfYear < _dateTime.Now)
        {
            var tillEndOfYearDays = (lastCheckedDateEndOfYear - lastCheckedDate).Days + DayCarriedForwardBetweenYear;
            if (DateTime.IsLeapYear(lastCheckedDateEndOfYear.Year))
            {
                investmentDays.LeapYearDays += tillEndOfYearDays;
            } else {
                investmentDays.RegularYearDays += tillEndOfYearDays;
            }

            return CalculateDaysYearly(investmentDays, new DateTime(lastCheckedDate.Year + 1, FirstMonthOfYear, FirstDayOfYear));
        }

        var days = (_dateTime.Now - lastCheckedDate).Days;

        if (DateTime.IsLeapYear(lastCheckedDate.Year))
        {
            investmentDays.LeapYearDays += days;
        } else {
            investmentDays.RegularYearDays += days;
        }

        return investmentDays;
    } 
}