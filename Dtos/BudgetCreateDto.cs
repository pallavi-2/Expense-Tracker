namespace ExpenseTracker.Dtos
{
    public class BudgetCreateDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal MonthlyLimit { get; set; }
        //public decimal AmountSpent { get; set; }
    }
}
