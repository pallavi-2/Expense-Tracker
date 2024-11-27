using ExpenseTracker.Data;
using ExpenseTracker.Dtos;
using ExpenseTracker.Mappers;
using ExpenseTracker.Models;
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
    public class SavingGoalsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SavingGoalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<GoalResponseDto>> GetGoals()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var goal = await _context.SavingGoals.Where(t => t.UserId == int.Parse(userId)).ToListAsync();
            if (goal == null)
            {
                return NotFound();
            }
            return Ok(goal);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGoal([FromBody] GoalCreateDto goalDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var goal = goalDto.ToGoalFromDto(int.Parse(userId));

            await _context.SavingGoals.AddAsync(goal);
            await _context.SaveChangesAsync();
            return Ok(goal);
        }
    }
}
