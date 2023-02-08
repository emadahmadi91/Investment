using Investment.Domain.Enums;

namespace Investment.Domain.Entities;

public class Investment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Principle { get; set; }
    public decimal Rate { get; set; }
    public DateTime StartDate { get; set; }
    public InvestmentType InvestmentType { get; set; }
}