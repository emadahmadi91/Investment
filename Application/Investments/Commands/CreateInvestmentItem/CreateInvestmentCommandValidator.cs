using FluentValidation;
using Investment.Application.Common.Interfaces;
using Investment.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Investment.Application.Investments.Commands.CreateInvestmentItem;

public class CreateInvestmentCommandValidator : AbstractValidator<CreateInvestmentCommand>
{
    private readonly IApplicationDbContext _context;
    
    public CreateInvestmentCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Should not be empty")
            .MustAsync(BeUniqueTitle).WithMessage("Name should be unique");
        
        RuleFor(v => v.Principle)
            .GreaterThan(0m).WithMessage("Should be a positive number");

        RuleFor(v => v.Rate)
            .GreaterThan(0m).WithMessage("Should be a positive number");

        RuleFor(v => v.StartDate)
            .Matches(@"\d{4}\-(0?[1-9]|1[012])\-(0?[1-9]|[12][0-9]|3[01])*")
            .WithMessage("Should be in yyyy-dd-MM format. eg 2000-12-21");
        
        RuleFor(v => v.InvestmentType)
            .IsEnumName(typeof(InvestmentType), caseSensitive: false)
            .WithMessage("Investment type is not of the supported types");
    }
    
    public async Task<bool> BeUniqueTitle(string name, CancellationToken cancellationToken)
    {
        return await _context.Investments
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
