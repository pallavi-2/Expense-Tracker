using ExpenseTracker.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OverviewController : ControllerBase
    {
        private readonly ApplicationDbContext _context; 
        public OverviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOverview()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); ;
            if (userId == null)
            {
                return Unauthorized();
            }
            var expenses = await _context.Transactions.Where(x => x.Type == "Expenses" && x.UserId == int.Parse(userId)).SumAsync(x => x.Amount);
            

            var income = await _context.Transactions.Where(x => x.Type == "Income" && x.UserId == int.Parse(userId)).SumAsync(x => x.Amount);

            var monthly = new OverviewDto { Expense = expenses, Income = income };
            return Ok(monthly);

        }
    }
}
