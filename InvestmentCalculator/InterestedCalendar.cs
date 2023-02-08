using Investment.Application.Common.Interfaces;
using Investment.Domain.Dto;

namespace Investment.InvestmentCalculator;

public class InvestmentCalendar : IInvestmentCalendar
{
    private const int LastMonthOfYear = 12;
    
    private const int LastDayOfYear = 31;
    
    private const int FirstMonthOfYear = 1;
    
    private const int FirstDayOfMonth = 1;
    
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

        if (startDate > DateTimeNowClipToTheClosestMonth())
            return investmentDays;
        
        return CalculateDaysYearly(investmentDays, startDate);
    }

    private InvestmentDays CalculateDaysYearly(InvestmentDays investmentDays, DateTime lastCheckedDate)
    {
        var lastCheckedDateEndOfYear = new DateTime(lastCheckedDate.Year, LastMonthOfYear, LastDayOfYear);
        
        if (lastCheckedDateEndOfYear < DateTimeNowClipToTheClosestMonth())
        {
            var tillEndOfYearDays = (lastCheckedDateEndOfYear - lastCheckedDate).Days + DayCarriedForwardBetweenYear;
            if (DateTime.IsLeapYear(lastCheckedDateEndOfYear.Year))
            {
                investmentDays.LeapYearDays += tillEndOfYearDays;
            } else {
                investmentDays.RegularYearDays += tillEndOfYearDays;
            }

            return CalculateDaysYearly(investmentDays, new DateTime(lastCheckedDate.Year + 1, FirstMonthOfYear, FirstDayOfMonth));
        }

        var days = (DateTimeNowClipToTheClosestMonth() - lastCheckedDate).Days;

        if (DateTime.IsLeapYear(lastCheckedDate.Year))
        {
            investmentDays.LeapYearDays += days;
        } else {
            investmentDays.RegularYearDays += days;
        }

        return investmentDays;
    }
    
    private DateTime DateTimeNowClipToTheClosestMonth()
    {
        var now = _dateTime.Now;
        
        return now.Day < DateTime.DaysInMonth(now.Year, now.Month) / 2 
            ? new DateTime(now.Year, now.Month, FirstDayOfMonth)
            : now.Month + 1 <= 12 
                ? new DateTime(now.Year, now.Month + 1, FirstDayOfMonth)
                : new DateTime(now.Year + 1, FirstMonthOfYear, FirstDayOfMonth);
    }
}