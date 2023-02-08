using FluentValidation.AspNetCore;
using Investment.Application.Common.Interfaces;
using Investment.Infrastructure.Services;
using Investment.InvestmentCalculator;
using Investment.WebApplication.Filters;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IInvestmentCalendar, InvestmentCalendar>();
        services.AddTransient<IInterestCalculator, InterestCalculator>();
        services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
            .AddFluentValidation(x => x.AutomaticValidationEnabled = false);
        
        return services;
    }
}
