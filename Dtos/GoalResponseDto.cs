namespace ExpenseTracker.Dtos
{
    public class GoalResponseDto
    {
        public int GoalId { get; set; }
        public string GoalName { get; set; }
        public float TotalAmount { get; set; }
        public float CollectedAmount { get; set; }
    }
}
