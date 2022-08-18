using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Bank.Core.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bank.Services.Authentication
{
    public class AuthDbContext : IdentityDbContext<BankUser>
    {
        private IDbContextTransaction _transaction;
        private bool IsTesting;

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<BankUser> BankUsers { get; set; }

        //public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var BankUsers = modelBuilder.Entity<BankUser>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public void Commit()
        {
            if (IsTesting) return;

            _transaction?.Commit();

            _transaction = null;

            SaveChanges();
        }

        public void BeginTransaction()
        {
            _transaction = _transaction ?? Database.BeginTransaction();
        }

        public void BeginTransactionTest()
        {
            _transaction = _transaction ?? Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void SetTestMode()
        {
            BeginTransactionTest();
            IsTesting = true;
        }
    }
}
