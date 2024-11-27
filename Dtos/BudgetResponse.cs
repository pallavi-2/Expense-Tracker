namespace ExpenseTracker.Dtos
{
    public class BudgetResponse
    {
        public int BudgetId {  get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal MonthlyLimit { get; set; }
        public decimal AmountSpent { get; set; }
    }
}
