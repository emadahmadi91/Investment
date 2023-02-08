using FluentValidation;
using Investment.Application.Common.Interfaces;
using Investment.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Investment.Application.Investments.Commands.UpdateInvestment;

public class UpdateInvestmentCommandValidator : AbstractValidator<UpdateInvestmentCommand>
{
    private readonly IApplicationDbContext _context;
    
    public UpdateInvestmentCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        
        RuleFor(v => v.UpdateInvestmentDto.Name)
            .NotEmpty().WithMessage("Should not be empty")
            .MustAsync(BeUniqueTitle).WithMessage("Name should be unique")
            .Unless(v => v.OldName.Equals(v.UpdateInvestmentDto.Name))
            .OverridePropertyName("Name");
        
        RuleFor(v => v.UpdateInvestmentDto.Principle)
            .GreaterThan(0m).WithMessage("Should be a positive number")
            .OverridePropertyName("Principle");
        
        RuleFor(v => v.UpdateInvestmentDto.Rate)
            .GreaterThan(0m).WithMessage("Should be a positive number")
            .OverridePropertyName("Rate");

        RuleFor(v => v.UpdateInvestmentDto.StartDate)
            .Matches(@"\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])*")
            .WithMessage("Should be in yyyy-dd-MM format. eg 2000-12-21")
            .OverridePropertyName("StartDate");
        
        RuleFor(v => v.UpdateInvestmentDto.InvestmentType)
            .IsEnumName(typeof(InvestmentType), caseSensitive: false)
            .WithMessage("Investment type is not of the supported types")
            .OverridePropertyName("InvestmentType");;
    }
    
    public async Task<bool> BeUniqueTitle(string name, CancellationToken cancellationToken)
    {
        return await _context.Investments
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
