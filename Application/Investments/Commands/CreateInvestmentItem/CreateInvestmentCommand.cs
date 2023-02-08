using Investment.Application.Common.Interfaces;
using Investment.Domain.Enums;
using MediatR;

namespace Investment.Application.Investments.Commands.CreateInvestmentItem;

using Domain.Entities;
public record CreateInvestmentCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    public decimal Principle { get; set; }
    public decimal Rate { get; set; }
    public DateTime StartDate { get; set; }
    public String Type { get; set; } = null!;
}

public class CreateInvestmentCommandHandler : IRequestHandler<CreateInvestmentCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateInvestmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateInvestmentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Investment()
        {
            Name = request.Name,
            Principle = request.Principle,
            Rate = request.Rate,
            StartDate = request.StartDate,
            Type = Enum.Parse<InvestmentType>(request.Type),
        };
        
        _context.Investments.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
