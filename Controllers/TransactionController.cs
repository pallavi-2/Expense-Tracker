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
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x=>x.TransactionId == id);
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
            //transaction.UserId = int.Parse(userId);
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            if (transaction.Type == "Expenses")
            {
                var category = transactionDto.Category;
                var categoryTransaction = await _context.Budgets.FirstOrDefaultAsync(x=>x.Category == category && x.UserId == int.Parse(userId));
                categoryTransaction.AmountSpent -= transactionDto.Amount;
                if (categoryTransaction.AmountSpent < 0)
                {
                    _emailSender.SendEmail(User.FindFirstValue(ClaimTypes.Email), String.Format("Budget limit for {0} has exceeded the limit", category));
                }
                await _context.SaveChangesAsync();


            }
            return Ok(transaction.ToTransactionDto());

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction([FromBody] TransactionCreateDto transactionDto, [FromRoute] int id)
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
    }
}
