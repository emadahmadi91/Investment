using Investment.Application.Common.Interfaces;

namespace Investment.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
