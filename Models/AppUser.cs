using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class AppUser
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public ICollection<Budget> Budgets { get; set; }
    }
}


