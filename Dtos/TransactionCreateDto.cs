namespace ExpenseTracker.Dtos
{
    public class TransactionCreateDto
    {

        public string Type { get; set; } = string.Empty; //Income or Expense
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
