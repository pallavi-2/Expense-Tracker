using ExpenseTracker.Data;
using ExpenseTracker.Dtos;
using ExpenseTracker.Interfaces;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseTracker.Controllers
    
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AccountController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody] RegisterDto userRegister)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userRegister.UserName || u.UserName == userRegister.Email);
            if (existingUser != null)
            {
                return Conflict("Username or email already exists.");
            }

            if (string.IsNullOrEmpty(userRegister.Password))
            {
                return BadRequest("Password is required.");
            }

            var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = userRegister.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userRegister.Password)),
                Email = userRegister.Email,
                PasswordSalt = hmac.Key,

            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserResponseDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserResponseDto>> Login(LoginDto userLogin)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == userLogin.Username);
            if (user == null)
            {
                return Unauthorized("Invalid Username");
            }

            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userLogin.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserResponseDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
