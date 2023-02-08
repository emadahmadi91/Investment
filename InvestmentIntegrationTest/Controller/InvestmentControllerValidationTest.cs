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
            InvestmentType = InvestmentType.Simple
        };
        await AddAsync(investment);
        var investmentDto = new InvestmentDto
        {
            Name = investment.Name,
            Principle = 1000m,
            Rate = 1.15m,
            StartDate = "2020-09-09",
            InvestmentType = "Simple"
        };
        
        var response = await GetClient().PostAsync("/api/Investments",
            new StringContent(JsonConvert.SerializeObject(investmentDto), Encoding.UTF8, "application/json"));
        
        response.Should().Be400BadRequest().And
            .OnlyHaveError("Name", "Name should be unique");
    }
    
    [Test]
    [TestCase("", 100, 1.2, "2020-09-09", "simple", "Name", "Should not be empty")]
    [TestCase(null, 100, 1.2, "2020-09-09", "simple", "Name", "The Name field is required.")]
    [TestCase("Name", -100, 1.2, "2020-09-09", "simple", "Principle", "Should be a positive number")]
    [TestCase("Name", null, 1.2, "2020-09-09", "simple", "Principle", "Should be a positive number")]
    [TestCase("Name", 100, -1.2, "2020-09-09", "simple", "Rate", "Should be a positive number")]
    [TestCase("Name", 100, null, "2020-09-09", "simple", "Rate", "Should be a positive number")]
    [TestCase("Name", 100, 1.2, "", "simple", "StartDate", "Should be in yyyy-dd-MM format. eg 2000-12-21")]
    [TestCase("Name", 100, 1.2, null, "simple", "StartDate", "The StartDate field is required.")]
    [TestCase("Name", 100, 1.2, "12312", "simple", "StartDate", "Should be in yyyy-dd-MM format. eg 2000-12-21")]
    [TestCase("Name", 100, 1.2, "12312", "simple", "StartDate", "Should be in yyyy-dd-MM format. eg 2000-12-21")]
    [TestCase("Name", 100, 1.2, "2020-09-09", "", "InvestmentType", "Investment type is not of the supported types")]
    [TestCase("Name", 100, 1.2, "2020-09-09", "unsupported", "InvestmentType", "Investment type is not of the supported types")]
    [TestCase("Name", 100, 1.2, "2020-09-09", null, "InvestmentType", "The InvestmentType field is required.")]
    public async Task ItReturnsBadRequestWithValidationErrorWhenCreatesNewInvestment(string name,
        decimal? principle, decimal? rate, string startDate, string investmentType, 
        string errorField, string message)
    {
        var investmentDto = new InvestmentDto();
        investmentDto.Name = name;
        if (principle != null)
            investmentDto.Principle = (decimal) principle;
        if (rate != null)
            investmentDto.Rate = (decimal) rate;
        investmentDto.StartDate = startDate;
        investmentDto.InvestmentType = investmentType;

        var response = await GetClient().PostAsync("/api/Investments",
            new StringContent(JsonConvert.SerializeObject(investmentDto), Encoding.UTF8, "application/json"));
        
        response.Should().Be400BadRequest().And
            .OnlyHaveError(errorField, message);
    }
    
    [Test]
    public async Task ItReturnsNotFoundWhenInvestmentNameDoesNotExistsToUpdate()
    {
        var investmentDto = new InvestmentDto
        {
            Name = "Name",
            Principle = 2000m,
            Rate = 2.15m,
            StartDate = "2021-09-09",
            InvestmentType = "Simple"
        };
        
        var response = await GetClient().PutAsync($"/api/Investments/Name",
            new StringContent(JsonConvert.SerializeObject(investmentDto), Encoding.UTF8, "application/json"));
        
        response.Should().Be404NotFound();
    }
    
    
    [Test]
    public async Task ItReturnsNotFoundWhenInvestmentNameDoesNotExistsToDelete()
    {
        var response = await GetClient().DeleteAsync($"/api/Investments/Name");
        
        response.Should().Be404NotFound();
    }
    
        [Test]
    [TestCase("", 100, 1.2, "2020-09-09", "simple", "Name", "Should not be empty")]
    [TestCase(null, 100, 1.2, "2020-09-09", "simple", "Name", "The Name field is required.")]
    [TestCase("Name", -100, 1.2, "2020-09-09", "simple", "Principle", "Should be a positive number")]
    [TestCase("Name", null, 1.2, "2020-09-09", "simple", "Principle", "Should be a positive number")]
    [TestCase("Name", 100, -1.2, "2020-09-09", "simple", "Rate", "Should be a positive number")]
    [TestCase("Name", 100, null, "2020-09-09", "simple", "Rate", "Should be a positive number")]
    [TestCase("Name", 100, 1.2, "", "simple", "StartDate", "Should be in yyyy-dd-MM format. eg 2000-12-21")]
    [TestCase("Name", 100, 1.2, null, "simple", "StartDate", "The StartDate field is required.")]
    [TestCase("Name", 100, 1.2, "12312", "simple", "StartDate", "Should be in yyyy-dd-MM format. eg 2000-12-21")]
    [TestCase("Name", 100, 1.2, "12312", "simple", "StartDate", "Should be in yyyy-dd-MM format. eg 2000-12-21")]
    [TestCase("Name", 100, 1.2, "2020-09-09", "", "InvestmentType", "Investment type is not of the supported types")]
    [TestCase("Name", 100, 1.2, "2020-09-09", "unsupported", "InvestmentType", "Investment type is not of the supported types")]
    [TestCase("Name", 100, 1.2, "2020-09-09", null, "InvestmentType", "The InvestmentType field is required.")]
    public async Task ItReturnsBadRequestWithValidationErrorWhenUpdateANewInvestment(string name,
        decimal? principle, decimal? rate, string startDate, string investmentType, 
        string errorField, string message)
    {
        var investment = new Investment
        {
            Name = "OldName",
            Principle = 1000m,
            Rate = 1.15m,
            StartDate = DateTime.Now,
            InvestmentType = InvestmentType.Simple
        };
        await AddAsync(investment);
        var investmentDto = new InvestmentDto();
        investmentDto.Name = name;
        if (principle != null)
            investmentDto.Principle = (decimal) principle;
        if (rate != null)
            investmentDto.Rate = (decimal) rate;
        investmentDto.StartDate = startDate;
        investmentDto.InvestmentType = investmentType;

        var response = await GetClient().PutAsync($"/api/Investments/{investment.Name}",
            new StringContent(JsonConvert.SerializeObject(investmentDto), Encoding.UTF8, "application/json"));
        
        response.Should().Be400BadRequest().And
            .OnlyHaveError(errorField, message);
    }

    [Test]
    public async Task ItReturnsShouldUpdateTheInvestmentWithTheSameNameAsBeforeWhenUpdateANewInvestment()
    {
        var investment = new Investment
        {
            Name = "Name",
            Principle = 1000m,
            Rate = 1.15m,
            StartDate = DateTime.Now,
            InvestmentType = InvestmentType.Simple
        };
        await AddAsync(investment);
        var investmentDto = new InvestmentDto
        {
            Name = investment.Name,
            Principle = 1m,
            Rate = 1m,
            StartDate = "2000-09-19",
            InvestmentType = "Compound"
        };

        var response = await GetClient().PutAsync($"/api/Investments/{investment.Name}",
            new StringContent(JsonConvert.SerializeObject(investmentDto), Encoding.UTF8, "application/json"));
        
        response.Should().Be204NoContent();
    }
    
}