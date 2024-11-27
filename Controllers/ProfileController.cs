using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Dtos;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
       public ProfileController(ApplicationDbContext context)
        {
            _context = context;           
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProfile([FromBody] RegisterDto userDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var hmac = new HMACSHA512();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
            user.PasswordSalt = hmac.Key;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
