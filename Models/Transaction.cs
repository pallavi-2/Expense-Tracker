namespace ExpenseTracker.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string Type { get; set; } = string.Empty; //Income or Expense
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int UserId { get; set; } 
        public AppUser User { get; set; }
    }
}
