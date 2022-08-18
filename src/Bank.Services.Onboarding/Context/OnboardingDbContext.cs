using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Bank.Services.Onboarding.Models;
using Bank.Services.Onboarding.Models.WaitingList;
using System.IO;

namespace Bank.Core.Context
{
    public class OnboardingDbContext : DbContext
    {
        private readonly IHostingEnvironment _env;
        private IDbContextTransaction _transaction;
        private bool IsTesting;

        public OnboardingDbContext() { }

        public OnboardingDbContext(IHostingEnvironment env) : base()
        {
            _env = env;
        }

        public DbSet<Prospect> Prospects { get; set; }
        public DbSet<WaitingList> WaitingLists { get; set; }
        public DbSet<WaitingListStatus> WaitingListStatuses { get; set; }
        public DbSet<CustomerWaitingListStatus> CustomerWaitingListStatuses { get; set; }
        public DbSet<CustomerWaitingList> CustomerWaitingLists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerWaitingListStatus>()
                        .HasKey(c => new { c.ProspectId, c.StatusId });

            modelBuilder.Entity<CustomerWaitingList>()
                        .HasKey(c => new { c.ProspectId, c.WaitingListId });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            if (_env != null && _env.IsDevelopment())
            {
                builder.AddUserSecrets(System.Reflection.Assembly.GetEntryAssembly());
            }

            var config = builder.Build();

            // define the database to use
            optionsBuilder.UseSqlServer(config.GetConnectionString(config["ActiveDbConnection"]));
        }

        public void Commit()
        {
            if (IsTesting)
            {
                SaveChanges();
                return;
            }

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
            _transaction = Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
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
