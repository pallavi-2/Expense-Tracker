using ExpenseTracker.Models;

namespace ExpenseTracker.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(AppUser user);
    }
}
