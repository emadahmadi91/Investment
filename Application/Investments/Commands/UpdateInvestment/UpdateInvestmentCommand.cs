using Investment.Application.Common.Exceptions;
using Investment.Application.Common.Interfaces;
using Investment.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Investment.Application.Investments.Commands.UpdateInvestment;

public record UpdateInvestmentCommand : IRequest
{
    public string OldName { get; set; }
    
    public UpdateInvestmentDTO UpdateInvestmentDto { get; set; }
} 
public record UpdateInvestmentDTO
{
    public string Name { get; set; } = null!;
    public decimal Principle { get; set; } 
    public decimal Rate { get; set; }
    public String StartDate { get; set; }
    public string InvestmentType { get; set; }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateInvestmentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateInvestmentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Investments
            .Where(n => n.Name == request.OldName)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Investment), request.OldName);

        entity.Name = request.UpdateInvestmentDto.Name;
        entity.Principle = request.UpdateInvestmentDto.Principle;
        entity.Rate = request.UpdateInvestmentDto.Rate;
        entity.StartDate = Convert.ToDateTime(request.UpdateInvestmentDto.StartDate);
        entity.InvestmentType = Enum.Parse<InvestmentType>(request.UpdateInvestmentDto.InvestmentType);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
