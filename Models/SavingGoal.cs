namespace ExpenseTracker.Models
{
    public class SavingGoal
    {
        public int GoalId { get; set; }
        public string GoalName { get; set; }
        public float TotalAmount { get; set; }
        public float CollectedAmount { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
