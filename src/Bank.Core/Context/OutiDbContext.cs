using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Bank.Core.Models;
using System;
using System.IO;

namespace Bank.Core.Context
{
    public class BankDbContext : DbContext
    {
        private readonly IHostingEnvironment _env;
        private IDbContextTransaction _transaction;
        private bool IsTesting;

        public BankDbContext(IHostingEnvironment env) : base()
        {
            _env = env;
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerStatus> CustomerStatus { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentStatus> DocumentStatus { get; set; }
        public DbSet<DocumentDetail> DocumentDetails { get; set; }
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<UploadType> UploadTypes { get; set; }
        public DbSet<OnboardingStep> OnboardingSteps { get; set; }
        public DbSet<OnboardingStepHistory> OnboardingStepHistories { get; set; }
        public DbSet<BankUser> BankUsers { get; set; }
        public DbSet<PhoneConfirmationCode> PhoneConfirmationCodes { get; set; }
        public DbSet<Phone> Phones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .Property(e => e.ZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(e => e.DocumentNumber)
                .IsUnicode(false);

            modelBuilder.Entity<CustomerStatus>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<DocumentType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Addresses);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Phones);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Documents);

            modelBuilder.Entity<Customer>()
                .HasOne(e => e.OnboardingStep);

            modelBuilder.Entity<CustomerStatusHistory>()
                        .HasKey(c => new { c.CustomerStatusId, c.CustomerId });

            modelBuilder.Entity<OnboardingStepHistory>()
                        .HasKey(c => new { c.OnboardingStepId, c.CustomerId });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            if (_env.IsDevelopment())
            {
                builder.AddUserSecrets(System.Reflection.Assembly.GetEntryAssembly());
            }

            var config = builder.Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString(config["ActiveDbConnection"]));
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
