using Investment.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Investment.Infrastructure.Persistence;

using Domain.Entities;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Investment> Investments => Set<Investment>();

}