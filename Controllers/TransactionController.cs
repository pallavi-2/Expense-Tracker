using ExpenseTracker.Data;
using ExpenseTracker.Dtos;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        public TransactionController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var transactions = await _context.Transactions
                                         .Where(t => t.UserId == int.Parse(userId))
                                         .ToListAsync();

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionResponse>> GetById([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x=>x.TransactionId == id && x.UserId==int.Parse(userId));
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction.ToTransactionDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateDto transactionDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the UserId from the token
            if (userId == null)
            {
                return Unauthorized();
            }

            if (transactionDto == null)
            {
                return NoContent();
            }

            var transaction = transactionDto.ToTransactionFromDto(int.Parse(userId));
            if (transaction.Type == "Expense")
            {
                var category = transactionDto.Category;
                var categoryTransaction = await _context.Budgets.FirstOrDefaultAsync(x => x.Category == category && x.UserId == int.Parse(userId));
                categoryTransaction.AmountSpent = categoryTransaction.AmountSpent + transactionDto.Amount;
                if (categoryTransaction.AmountSpent > categoryTransaction.MonthlyLimit)
                {
                    _emailSender.SendEmail(User.FindFirstValue(ClaimTypes.Email),"Monthly Limit Reached", String.Format("Budget limit for {0} has exceeded the limit", category));
                }
            }

            if (transaction.Type == "Goal")
            {
                var category = transactionDto.Category;
                var goalTransaction = await _context.SavingGoals.FirstOrDefaultAsync(x => x.GoalName == category && x.UserId == int.Parse(userId));
                goalTransaction.CollectedAmount += (float)transactionDto.Amount;
                if (goalTransaction.CollectedAmount > goalTransaction.TotalAmount)
                {
                    _emailSender.SendEmail(User.FindFirstValue(ClaimTypes.Email),"Goal Reached", String.Format("You have reached the goal amount for {0}.", goalTransaction.GoalName));
                }
            }

            await _context.Transactions.AddAsync(transaction);


            await _context.SaveChangesAsync();
            return Ok(transaction.ToTransactionDto());

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TransactionResponse>> UpdateTransaction([FromBody] TransactionCreateDto transactionDto, [FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.TransactionId == id && x.UserId == int.Parse(userId));
            if (transaction == null)
            {
                return NotFound();
            }
            transaction.Type = transactionDto.Type;
            transaction.Amount = transactionDto.Amount;
            transaction.Category = transactionDto.Category;
            transaction.Description = transactionDto.Description;

            await _context.SaveChangesAsync();
            return Ok(transaction.ToTransactionDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.TransactionId == id && x.UserId == int.Parse(userId));
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return (NoContent());

        }

        [HttpGet("aggregate")]
        public async Task<IActionResult> GetSpendingByTimePeriod([FromQuery] string period)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            DateTime dateTime;
            switch (period.ToLower())
            {
                case "weekly": { dateTime = DateTime.UtcNow.AddDays(-7); break; }
                case "monthly": { dateTime = DateTime.UtcNow.AddMonths(-1); break; }
                case "yearly": { dateTime = DateTime.UtcNow.AddYears(-1); break; }
                default:
                    return BadRequest("Invalid period. Use 'weekly', 'monthly', or 'yearly'.");

            }

            var transaction = await _context.Transactions.Where(x=>x.UserId == int.Parse(userId) && x.Date >= dateTime)
                .GroupBy(x=>x.Type)
                .Select(g => new
                {
                    Type = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .ToListAsync();

            return Ok(transaction);

        }
    }
}
