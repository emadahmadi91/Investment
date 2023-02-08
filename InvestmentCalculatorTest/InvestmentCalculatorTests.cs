using FluentAssertions;
using Investment.Application.Common.Interfaces;
using Moq;
using Investment.Domain.Dto;
using Investment.InvestmentCalculator;

namespace InvestmentCalculatorTest;

public class InvestmentCalculatorTests
{
    private Mock<IInvestmentCalendar> _investmendCalendar;

    private InterestedCalculator _interestCalculator;

    private const decimal NumberOfDaysInAYear = 365m;
    private const decimal NumberOfDaysInALeapYear = 366m;

    [SetUp]
    public void Setup()
    {
        _investmendCalendar = new Mock<IInvestmentCalendar>();
        _interestCalculator = new InterestedCalculator(_investmendCalendar.Object);
    }

    [Test]
    [TestCase(10000, 1, 100)]
    [TestCase(10000, 1.5, 150)]
    [TestCase(50.51, 1, 0.51)]
    [TestCase(50.5, 1.5, 0.76)]
    [TestCase(50.99, 1.5, 0.76)]
    [TestCase(51.01, 1.5, 0.77)]
    [TestCase(51.66, 1.5, 0.77)]
    [TestCase(51.67, 1.5, 0.78)]
    public void GivenInvestmentIsSimpleWhenInterestIsCalculatedAfterAYearThenExpectedInterestIsReturned(
        decimal principle, decimal rate, decimal expectedValue)
    {
        _investmendCalendar.Setup(investmentCalendar => investmentCalendar.GetInvestmentDays(It.IsAny<InvestmentDto>()))
            .Returns(new InvestmentDays { RegularYearDays = NumberOfDaysInAYear, LeapYearDays = 0m });
        var investment = new InvestmentDto { Principle = principle, Rate = rate, InvestmentType = "Simple" };

        var interest = _interestCalculator.CalculateInterest(investment);

        interest.Should().Be(expectedValue);
    }
    
    [Test]
    [TestCase(10000, 1, 100)]
    [TestCase(10000, 1.5, 150)]
    [TestCase(50.51, 1, 0.51)]
    [TestCase(50.5, 1.5, 0.76)]
    [TestCase(50.99, 1.5, 0.76)]
    [TestCase(51.01, 1.5, 0.77)]
    [TestCase(51.66, 1.5, 0.77)]
    [TestCase(51.67, 1.5, 0.78)]
    public void GivenInvestmentIsSimpleWhenInterestIsCalculatedAfterALeapYearThenExpectedInterestIsReturned(
        decimal principle, decimal rate, decimal expectedValue)
    {
        _investmendCalendar.Setup(investmentCalendar => investmentCalendar.GetInvestmentDays(It.IsAny<InvestmentDto>()))
            .Returns(new InvestmentDays { RegularYearDays = 0m, LeapYearDays = NumberOfDaysInALeapYear });
        var investment = new InvestmentDto { Principle = principle, Rate = rate, InvestmentType = "Simple" };

        var interest = _interestCalculator.CalculateInterest(investment);

        interest.Should().Be(expectedValue);
    }

    [Theory]
    [TestCase(10000, 1, 8.33)]
    [TestCase(10000, 1.5, 12.50)]
    [TestCase(50.5, 1, 0.04)]
    [TestCase(52.00, 1.5, 0.06)]
    [TestCase(52.01, 1.5, 0.07)]
    [TestCase(5, 1.5, 0.01)]
    [TestCase(3, 1.5, 0.00)]
    public void GivenInvestmentIsSimpleWhenInterestIsCalculatedAfterOneMonthExpectedInterestIsReturned(
        decimal principle, decimal rate, decimal expectedValue)
    {
        _investmendCalendar.Setup(investmentCalendar =>
                investmentCalendar.GetInvestmentDays(It.IsAny<InvestmentDto>()))
            .Returns(new InvestmentDays { RegularYearDays = NumberOfDaysInAYear / 12m, LeapYearDays = 0m });
        var investment = new InvestmentDto { Principle = principle, Rate = rate, InvestmentType = "Simple" };

        var interest = _interestCalculator.CalculateInterest(investment);

        interest.Should().Be(expectedValue);
    }
    
    [Theory]
    [TestCase(10000, 1, 8.33)]
    [TestCase(10000, 1.5, 12.50)]
    [TestCase(50.5, 1, 0.04)]
    [TestCase(51.99, 1.5, 0.06)]
    [TestCase(52.00, 1.5, 0.07)]
    [TestCase(5, 1.5, 0.01)]
    [TestCase(3, 1.5, 0.00)]
    public void GivenInvestmentIsSimpleWhenInterestIsCalculatedAfterOneMonthInLeapExpectedInterestIsReturned(
        decimal principle, decimal rate, decimal expectedValue)
    {
        _investmendCalendar.Setup(investmentCalendar =>
                investmentCalendar.GetInvestmentDays(It.IsAny<InvestmentDto>()))
            .Returns(new InvestmentDays { RegularYearDays = 0m, LeapYearDays = NumberOfDaysInALeapYear / 12m });
        var investment = new InvestmentDto { Principle = principle, Rate = rate, InvestmentType = "Simple" };

        var interest = _interestCalculator.CalculateInterest(investment);

        interest.Should().Be(expectedValue);
    }

    [Test]
    [TestCase(10000, 1, 100.50)]
    [TestCase(10000, 1.5, 151.13)]
    [TestCase(50.10, 1.5, 0.76)]
    [TestCase(5001.10, 1.57, 79.14)]
    [TestCase(50.5, 1.5, 0.76)]
    [TestCase(50.99, 1.51, 0.78)]
    [TestCase(51.01, 1.5, 0.77)]
    [TestCase(51.66, 1.5, 0.78)]
    [TestCase(51.67, 1.5, 0.78)]
    public void GivenInvestmentIsCompoundWhenInterestIsCalculatedAfterAYearThenExpectedInterestIsReturned(
        decimal principle, decimal rate, decimal expectedValue)
    {
        _investmendCalendar.Setup(investmentCalendar =>
                investmentCalendar.GetInvestmentDays(It.IsAny<InvestmentDto>()))
            .Returns(new InvestmentDays { RegularYearDays = NumberOfDaysInAYear, LeapYearDays = 0m });
        var investment = new InvestmentDto { Principle = principle, Rate = rate, InvestmentType = "Compound" };

        var interest = _interestCalculator.CalculateInterest(investment);

        interest.Should().Be(expectedValue);
    }
    
    [Test]
    [TestCase(10000, 1, 100.50)]
    [TestCase(10000, 1.5, 151.13)]
    [TestCase(50.10, 1.5, 0.76)]
    [TestCase(5001.10, 1.57, 79.14)]
    [TestCase(50.5, 1.5, 0.76)]
    [TestCase(50.99, 1.51, 0.78)]
    [TestCase(51.01, 1.5, 0.77)]
    [TestCase(51.66, 1.5, 0.78)]
    [TestCase(51.67, 1.5, 0.78)]
    public void GivenInvestmentIsCompoundWhenInterestIsCalculatedAfterALeapYearThenExpectedInterestIsReturned(
        decimal principle, decimal rate, decimal expectedValue)
    {
        _investmendCalendar.Setup(investmentCalendar =>
                investmentCalendar.GetInvestmentDays(It.IsAny<InvestmentDto>()))
            .Returns(new InvestmentDays { RegularYearDays = 0m, LeapYearDays = NumberOfDaysInALeapYear });
        var investment = new InvestmentDto { Principle = principle, Rate = rate, InvestmentType = "Compound" };

        var interest = _interestCalculator.CalculateInterest(investment);

        interest.Should().Be(expectedValue);
    }
    
    [Test]
    [TestCase(10000, 1, 8.34)]
    [TestCase(10000, 1.5, 12.51)]
    [TestCase(50.10, 1.5, 0.06)]
    [TestCase(5001.10, 1.57, 6.55)]
    [TestCase(50.5, 1, 0.04)]
    [TestCase(50.99, 1.5, 0.06)]
    [TestCase(52.00, 1.5, 0.07)]
    [TestCase(5, 1.5, 0.01)]
    [TestCase(3, 1.5, 0.00)]
    public void GivenInvestmentIsCompoundWhenInterestIsCalculatedAfterOneMonthInLeapYearThenExpectedInterestIsReturned(
        decimal principle, decimal rate, decimal expectedValue)
    {
        _investmendCalendar.Setup(investmentCalendar =>
                investmentCalendar.GetInvestmentDays(It.IsAny<InvestmentDto>()))
            .Returns(new InvestmentDays { RegularYearDays = 0m, LeapYearDays = NumberOfDaysInALeapYear / 12m });
        var investment = new InvestmentDto { Principle = principle, Rate = rate, InvestmentType = "Compound" };

        var interest = _interestCalculator.CalculateInterest(investment);

        interest.Should().Be(expectedValue);
    }

    [Test]
    [TestCase(10000, 1, 8.34)]
    [TestCase(10000, 1.5, 12.51)]
    [TestCase(50.10, 1.5, 0.06)]
    [TestCase(5001.10, 1.57, 6.55)]
    [TestCase(50.5, 1, 0.04)]
    [TestCase(50.99, 1.5, 0.06)]
    [TestCase(52.00, 1.5, 0.07)]
    [TestCase(5, 1.5, 0.01)]
    [TestCase(3, 1.5, 0.00)]
    public void GivenInvestmentIsCompoundWhenInterestIsCalculatedAfterOneMonthThenExpectedInterestIsReturned(
        decimal principle, decimal rate, decimal expectedValue)
    {
        _investmendCalendar.Setup(investmentCalendar =>
                investmentCalendar.GetInvestmentDays(It.IsAny<InvestmentDto>()))
            .Returns(new InvestmentDays { RegularYearDays = NumberOfDaysInAYear / 12m, LeapYearDays = 0m });
        var investment = new InvestmentDto { Principle = principle, Rate = rate, InvestmentType = "Compound" };

        var interest = _interestCalculator.CalculateInterest(investment);

        interest.Should().Be(expectedValue);
    }
}