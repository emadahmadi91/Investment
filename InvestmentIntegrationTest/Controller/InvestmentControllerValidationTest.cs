using System.Net;
using System.Text;
using FluentAssertions;
using Investment.Domain.Dto;
using Investment.Domain.Enums;
using Newtonsoft.Json;
using NUnit.Framework;

namespace InvestmentIntegrationTest.Controller;

using Investment.Domain.Entities;

using static Testing;

public class InvestmentControllerValidationTest : BaseTestFixture
{
    [Test]
    public async Task ItReturnsBadRequestIfInvestmentNameIsNotUniqueWhenCreatesNewInvestment()
    {
        var investment = new Investment
        {
            Name = "Name",
            Principle = 1000m,
            Rate = 1.15m,
            StartDate = DateTime.Now,
            Type = InvestmentType.Simple
        };
        await AddAsync(investment);
        var investmentDto = new InvestmentDto
        {
            Name = investment.Name,
            Principle = 1000m,
            Rate = 1.15m,
            StartDate = "2020-09-09",
            Type = "Simple"
        };
        
        var response = await GetClient().PostAsync("/api/Investments",
            new StringContent(JsonConvert.SerializeObject(investmentDto), Encoding.UTF8, "application/json"));
        
        response.Should().Be400BadRequest().And
            .OnlyHaveError("Name", "Name should be unique");
    }
    
}