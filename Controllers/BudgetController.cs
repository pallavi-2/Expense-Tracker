using ExpenseTracker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Dtos;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Mappers;

namespace ExpenseTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BudgetController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetResponse>>> GetBudget()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var budget = await _context.Budgets.Where(t => t.UserId == int.Parse(userId))
                                         .ToListAsync();
            if (budget == null)
            {
                return NotFound();
            }
            return Ok(budget);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetResponse>> GetBudgetById([FromRoute] int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var budget = await _context.Budgets.FirstOrDefaultAsync(x=>x.BudgetId == id && x.UserId == int.Parse(userId));
            if (budget == null)
            {
                return NotFound();
            }
            return (budget.ToBudgetDto());

        }

        [HttpPost]
        public async Task<IActionResult> CreateBudget([FromBody] BudgetCreateDto budgetDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            if (budgetDto == null)
            {
                return NoContent();
            }

            var budget = budgetDto.FromDtoToBudget(int.Parse(userId));
            await _context.Budgets.AddAsync(budget);
            await _context.SaveChangesAsync();
            return Ok(budget.ToBudgetDto());
        
        }
    }
}
