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
    
    [Test]
    public async Task ItGetsInvestments()
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
        
        var response = await GetClient().GetAsync("/api/Investments");
        
        response.EnsureSuccessStatusCode();
        var items = JsonConvert.DeserializeObject<List<InvestmentDto>>(await response.Content.ReadAsStringAsync());
        items.Count.Should().Be(1);
        items[0].Should().NotBeNull();
        items[0].Name.Should().BeEquivalentTo(investment.Name);
        items[0].Principle.Should().BeApproximately(investment.Principle, 00.1m);
        items[0].Rate.Should().BeApproximately(investment.Rate, 00.1m);
        items[0].StartDate.Should().Be(investment.StartDate.ToString("yyyy-dd-MM"));
        items[0].Type.Should().Be(investment.Type.ToString());
    }
    
    [Test]
    public async Task ItDeletesInvestments()
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
        
        var response = await GetClient().DeleteAsync($"/api/Investments/{investment.Name}");
        
        response.EnsureSuccessStatusCode();
        (await CountAsync<Investment>()).Should().Be(0);
    }
    
    [Test]
    public async Task ItUpdatesInvestments()
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
            Name = "new" + investment.Name,
            Principle = 2000m,
            Rate = 2.15m,
            StartDate = "2021-09-09",
            Type = "Simple"
        };
        
        var response = await GetClient().PutAsync($"/api/Investments/{investment.Name}",
            new StringContent(JsonConvert.SerializeObject(investmentDto), Encoding.UTF8, "application/json"));
        
        response.EnsureSuccessStatusCode();
        var item = await FindBy<Investment>(i => i.Name == investmentDto.Name);
        item.Should().NotBeNull();
        item.Name.Should().BeEquivalentTo(investmentDto.Name);
        item.Principle.Should().BeApproximately(investmentDto.Principle, 00.1m);
        item.Rate.Should().BeApproximately(investmentDto.Rate, 00.1m);
        item.StartDate.ToString("yyyy-dd-MM").Should().BeEquivalentTo(investmentDto.StartDate);
        item.Type.Should().Be(Enum.Parse<InvestmentType>(investmentDto.Type));
    }
}