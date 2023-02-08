using Investment.Application.Common.Interfaces;
using Investment.Domain.Dto;

namespace Investment.InvestmentCalculator;

public class InterestedCalculator : IInvestmentCalculator
{
    private readonly IInvestmentCalendar _investmentCalendar;
         
    private const decimal NumberOfDaysInAYear = 365;
    
    private const decimal NumberOfDaysInALeapYear = 366;

    public InterestedCalculator(IInvestmentCalendar calendar)
    {
        _investmentCalendar = calendar;
    }

    public decimal CalculateInterest(InvestmentDto investment)
    {
        var interest = investment.InvestmentType == "Simple" 
            ? CalculateForSimpleInvestment(investment) 
            : CalculateForCompoundInvestment(investment);
             
        return Math.Round(interest, 2, MidpointRounding.AwayFromZero);
    }

    private decimal CalculateForSimpleInvestment(InvestmentDto investment)
    {
        var investmentDays = _investmentCalendar.GetInvestmentDays(investment);
        var rateInPercentage = investment.Rate / 100;
        
        var ratePerDay = rateInPercentage / NumberOfDaysInAYear;
        var interestInRegularYear = investmentDays.RegularYearDays * ratePerDay * investment.Principle;
        
        var ratePerLeapYearDay = rateInPercentage / NumberOfDaysInALeapYear;
        var interestInLeapYear = investmentDays.LeapYearDays * ratePerLeapYearDay * investment.Principle;
        
        return interestInRegularYear + interestInLeapYear;
    }

    private decimal CalculateForCompoundInvestment(InvestmentDto investment)
    {
        var investmentDays = _investmentCalendar.GetInvestmentDays(investment);
        var rateInPercentage = investment.Rate / 100;

        var interestInRegularYear = CompoundInterestCalculator(rateInPercentage, NumberOfDaysInAYear,
            investmentDays.RegularYearDays, investment.Principle);
        
        var interestInLeapYear = CompoundInterestCalculator(rateInPercentage, NumberOfDaysInALeapYear,
            investmentDays.LeapYearDays, investment.Principle);

        return interestInRegularYear + interestInLeapYear;
    }


    private decimal CompoundInterestCalculator(decimal rateInPercentage, decimal numberOfDaysInYear,
        decimal numberOfDaysInvestedInPeriod, decimal principle)
    {
        decimal ratePerDay = rateInPercentage / numberOfDaysInYear;
        var compoundingPeriodPerDay = (decimal)Math.Pow(decimal.ToDouble(ratePerDay + 1),
            decimal.ToDouble(numberOfDaysInvestedInPeriod));
        return compoundingPeriodPerDay * principle - principle;
    }
}