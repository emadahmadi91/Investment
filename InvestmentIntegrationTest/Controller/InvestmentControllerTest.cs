using System.Text;
using Investment.Domain.Dto;
using Newtonsoft.Json;
using NUnit.Framework;

namespace InvestmentIntegrationTest.Controller;

using static Testing;

public class InvestmentControllerTest : BaseTestFixture
{
    [Test]
    public async Task ItCreatesNewInvestment()
    {
        // Arrange
        var investmentDto = new InvestmentDto
        {
            Name = "Name",
            Principle = 1000m,
            Rate = 1.15m,
            StartDate = "2020-09-09",
            Type = "Simple"
        };
        
        // Act
        var response = await GetClient().PostAsync("/api/Investments",
            new StringContent(JsonConvert.SerializeObject(investmentDto), Encoding.UTF8, "application/json"));
        
        // Assert
        response.EnsureSuccessStatusCode();
    }
}