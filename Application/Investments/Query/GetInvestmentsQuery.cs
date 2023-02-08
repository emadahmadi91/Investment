using Investment.Application.Common.Interfaces;
using Investment.Domain.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Investment.Application.Investments.Query;

public record GetInvestmentsQuery : IRequest<List<InvestmentDto>>
{ }

public class GetInvestmentsHandler : IRequestHandler<GetInvestmentsQuery, List<InvestmentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;


    public GetInvestmentsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<InvestmentDto>> Handle(GetInvestmentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Investments
            .OrderBy(x => x.Id)
            .ProjectTo<InvestmentDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}

