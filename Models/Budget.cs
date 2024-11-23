namespace ExpenseTracker.Models
{
    public class Budget
    {
        public int BudgetId { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal MonthlyLimit { get; set; }
        public decimal AmountSpent { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
