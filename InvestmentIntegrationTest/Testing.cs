using System.Linq.Expressions;
using System.Net.Http.Headers;
using Investment.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace InvestmentIntegrationTest;

[SetUpFixture]
public partial class Testing
{
    private static WebApplicationFactory<Program> _factory = null!;
    private static IConfiguration _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        _factory = new CustomWebApplicationFactory();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        _configuration = _factory.Services.GetRequiredService<IConfiguration>();
    }
    public static HttpClient GetClient()
    {
        var httpClient = _factory.CreateClient();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return httpClient;
    }
    
    public static async Task<TEntity?> FindBy<TEntity>(Expression<Func<TEntity,bool>> expression) where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        return await context.Set<TEntity>().Where(expression).FirstOrDefaultAsync();
    }
    
    public static async Task ResetState()
    {
        using var scope = _scopeFactory.CreateScope();

        await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database
            .EnsureDeletedAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}
