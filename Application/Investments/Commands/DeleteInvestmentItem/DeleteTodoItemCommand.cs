using Investment.Application.Common.Exceptions;
using Investment.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Investment.Application.Investments.Commands.DeleteInvestmentItem;

public record DeleteTodoItemCommand(string Name) : IRequest;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Investments
            .Where(n => n.Name == request.Name)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
            throw new NotFoundException(nameof(Investment), request.Name);
        
        _context.Investments.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
