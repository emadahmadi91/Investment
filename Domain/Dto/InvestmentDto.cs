namespace Investment.Domain.Dto
{
    public class InvestmentDto
    {
        public string Name { get; set; } = null!;
        public decimal Principle { get; set; }
        public decimal Rate { get; set; }
        public string StartDate { get; set; } = null!;
        public string InvestmentType { get; set; } = null!;
        
        public decimal Value { get; set; }
    }
}