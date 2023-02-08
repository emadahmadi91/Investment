using Microsoft.EntityFrameworkCore;

namespace Investment.Application.Common.Interfaces;

using Domain.Entities;

public interface IApplicationDbContext
{
    DbSet<Investment> Investments { get; }

}
