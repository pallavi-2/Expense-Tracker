using ExpenseTracker.Dtos;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappers
{
    public static class BudgetMapper
    {
        public static BudgetResponse ToBudgetDto(this Budget budgetDto)
        {
            return new BudgetResponse
            {
                BudgetId = budgetDto.BudgetId,
                Category = budgetDto.Category,
                MonthlyLimit = budgetDto.MonthlyLimit,
                AmountSpent = budgetDto.AmountSpent,
            };
        }

        public static Budget FromDtoToBudget(this BudgetCreateDto budgetDto , int UserId)
        {
            return new Budget
            {
                Category = budgetDto.Category,
                MonthlyLimit = budgetDto.MonthlyLimit,
                AmountSpent = 0,
                UserId = UserId
            };
            }
    }
}
