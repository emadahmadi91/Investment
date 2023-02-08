namespace Investment.Domain.Dto
{
    public class InvestmentDto
    {
        public string Name { get; set; } = null!;
        public decimal Principle { get; set; }
        public decimal Rate { get; set; }
        public String StartDate { get; set; } = null!;
        public String Type { get; set; } = null!;
        
        public decimal Value { get; set; }
    }
}