using ExpenseTracker.Dtos;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappers
{
    public static class GoalMapper
    {
        public static SavingGoal ToGoalFromDto(this GoalCreateDto goalDto, int userId)
        {
            return new SavingGoal
            {
                GoalName = goalDto.GoalName,
                TotalAmount = goalDto.TotalAmount,
                CollectedAmount = goalDto.CollectedAmount,
                UserId = userId
            };
        }
        public static GoalResponseDto ToGoalDto(this SavingGoal goal)
        {
            return new GoalResponseDto
            {
                GoalId = goal.GoalId,
                GoalName = goal.GoalName,
                TotalAmount = goal.TotalAmount,
                CollectedAmount = goal.CollectedAmount,
            };
        }
    }

    
}
