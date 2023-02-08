using Investment.Application.Common.Interfaces;
using Investment.Domain.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Investment.Application.Common.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Investment.Application.Investments.Query;

public record GetInvestmentsQuery : IRequest<List<InvestmentDto>>
{ }

public class GetInvestmentsHandler : IRequestHandler<GetInvestmentsQuery, List<InvestmentDto>>
{
    private readonly IApplicationDbContext _context;
    
    private readonly IMapper _mapper;
    
    private readonly IInterestCalculator _interestCalculator;
    
    public GetInvestmentsHandler(IApplicationDbContext context, IMapper mapper, IInterestCalculator interestCalculator)
    {
        _context = context;
        _mapper = mapper;
        _interestCalculator = interestCalculator;
    }

    public async Task<List<InvestmentDto>> Handle(GetInvestmentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Investments
            .OrderBy(x => x.Id)
            .ProjectTo<InvestmentDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken)
            .CalculateInvestment(_interestCalculator)
            ;
    }
}

