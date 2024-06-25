using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialManagementApp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinancialManagementApp.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Transaction entity
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            });


            modelBuilder.Entity<Transfer>(entity =>
                {
                    entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                    entity.HasOne(t => t.SenderAccount)
                        .WithMany()
                        .HasForeignKey(t => t.SenderAccountId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);

                    entity.HasOne(t => t.RecipientAccount)
                        .WithMany()
                        .HasForeignKey(t => t.RecipientAccountId)
                        .IsRequired()
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Balance).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Currency)
                      .HasConversion(
                          v => v.ToString(),
                          v => (Currency)Enum.Parse(typeof(Currency), v)
                      );
                entity.HasOne(a => a.AppUser)
                      .WithMany(u => u.Accounts)
                      .HasForeignKey(a => a.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasIndex(a => a.AccountName).IsUnique();
            });

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
