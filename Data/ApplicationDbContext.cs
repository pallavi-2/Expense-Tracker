using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<SavingGoal> SavingGoals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User) // Transaction has one User
                .WithMany(u => u.Transactions) // User has many Transactions
                .HasForeignKey(t => t.UserId) // The foreign key in Transaction
                .OnDelete(DeleteBehavior.Cascade); // Optional: Configure cascade delete

            modelBuilder.Entity<Budget>()
                .HasOne(t=>t.User)
                .WithMany(u=>u.Budgets)
                .HasForeignKey(t => t.UserId) 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SavingGoal>()
                .HasOne(t => t.User)
                .WithMany(u => u.SavingGoals)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
