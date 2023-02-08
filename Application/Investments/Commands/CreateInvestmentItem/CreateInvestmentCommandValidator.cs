using FluentValidation;
using Investment.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Investment.Application.Investments.Commands.CreateInvestmentItem;

public class CreateInvestmentCommandValidator : AbstractValidator<CreateInvestmentCommand>
{
    private readonly IApplicationDbContext _context;
    
    public CreateInvestmentCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        
        RuleFor(v => v.Name)
            .NotEmpty()
            .MustAsync(BeUniqueTitle).WithMessage("Name should be unique");
    }
    
    public async Task<bool> BeUniqueTitle(string name, CancellationToken cancellationToken)
    {
        return await _context.Investments
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
