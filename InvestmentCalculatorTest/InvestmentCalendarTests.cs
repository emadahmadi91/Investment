using FluentAssertions;
using Investment.Application.Common.Interfaces;
using Investment.Domain.Dto;
using Investment.InvestmentCalculator;
using Moq;

namespace InvestmentCalculatorTest;

public class InvestmentCalendarTests
{
    private InvestmentCalendar _investmentCalendar;
    
    private Mock<IDateTime> _dateTimeMock;

    [SetUp]
    public void Setup()
    {
        _dateTimeMock = new Mock<IDateTime>();
        _investmentCalendar = new InvestmentCalendar(_dateTimeMock.Object);
    }
    
    [Test]
    [TestCase("2022-02-10", "2022-02-09", 1, 0)]
    [TestCase("2022-12-31", "2022-01-01", 364, 0)]
    [TestCase("2023-01-01", "2022-01-01", 365, 0)]
    [TestCase("2024-01-01", "2021-01-01", 1095, 0)]
    [TestCase("2022-02-09", "2022-02-10", 0, 0)]
    [TestCase("2022-02-10", "2022-01-09", 32, 0)]
    [TestCase("2022-01-09", "2021-01-01", 373, 0)]
    [TestCase("2020-01-01", "2020-01-09", 0, 0)]
    [TestCase("2020-01-09", "2020-01-01", 0, 8)]
    [TestCase("2020-12-31", "2020-01-01", 0, 365)]
    [TestCase("2020-01-09", "2019-12-01", 31, 8)]
    public void ItProvidesTheNumberOfDates(string currentDate, string startDate, int regularYearDays, int leapYearDays)
    {
        _dateTimeMock.Setup(dateTime => dateTime.Now)
            .Returns(DateTime.Parse(currentDate));
        var investment = new InvestmentDto { StartDate = startDate };

        var investmentDays = _investmentCalendar.GetInvestmentDays(investment);

        investmentDays.RegularYearDays.Should().Be(regularYearDays);
        investmentDays.LeapYearDays.Should().Be(leapYearDays);
    }
    
    
    

}