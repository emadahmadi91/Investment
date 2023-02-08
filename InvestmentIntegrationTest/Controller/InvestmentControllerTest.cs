using System.Text;
using FluentAssertions;
using Investment.Domain.Dto;
using Investment.Domain.Enums;
using Newtonsoft.Json;
using NUnit.Framework;

namespace InvestmentIntegrationTest.Controller;

using Investment.Domain.Entities;

using static Testing;

public class InvestmentControllerTest : BaseTestFixture
{
    [Test]
    public async Task ItCreatesNewInvestment()
    {
        var investmentDto = new InvestmentDto
        {
            Name = "Name",
            Principle = 1000m,
            Rate = 1.15m,
            StartDate = "2020-09-09",
            Type = "Simple"
        };
        
        var response = await GetClient().PostAsync("/api/Investments",
            new StringContent(JsonConvert.SerializeObject(investmentDto), Encoding.UTF8, "application/json"));
        
        response.EnsureSuccessStatusCode();
        var item = await FindBy<Investment>(i => i.Name == "Name");
        item.Should().NotBeNull();
        item.Name.Should().BeEquivalentTo(investmentDto.Name);
        item.Principle.Should().BeApproximately(investmentDto.Principle, 00.1m);
        item.Rate.Should().BeApproximately(investmentDto.Rate, 00.1m);
        item.StartDate.ToString("yyyy-dd-MM").Should().BeEquivalentTo(investmentDto.StartDate);
        item.Type.Should().Be(Enum.Parse<InvestmentType>(investmentDto.Type));
    }
}