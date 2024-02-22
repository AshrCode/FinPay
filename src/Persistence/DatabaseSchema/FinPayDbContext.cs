using Microsoft.EntityFrameworkCore;
using Entities = Domain.Entities;

namespace Persistence.DatabaseSchema
{
    public class FinPayDbContext : DbContext
    {
        public FinPayDbContext(DbContextOptions<FinPayDbContext> options)
            : base(options) { }

        #region DbSet

        public DbSet<Entities.User> Users { get; set; }

        public DbSet<Entities.Beneficiary> Beneficiaries { get; set; }

        public DbSet<Entities.Transaction> Transactions { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Beneficiary
            modelBuilder.Entity<Entities.Beneficiary>()
                .HasOne<Entities.User>()
                .WithMany().HasForeignKey(x => x.UserId);

            // Transaction
            modelBuilder.Entity<Entities.Transaction>()
                .HasOne<Entities.User>()
                .WithMany().HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Entities.Transaction>()
                .HasOne<Entities.Beneficiary>()
                .WithMany().HasForeignKey(x => x.BeneficiaryId);

                // Seeding User data
            modelBuilder.Entity<Entities.User>().HasData(
                new Entities.User
                {
                    Id = new Guid("B136CF3D-766B-45AE-AA84-AC7F10C5A090"),
                    IsVerified = true
                },
                new Entities.User
                {

                    Id = new Guid("6751304E-0EEA-443C-AD6A-DFBBF53731FE"),
                    IsVerified = false
                }
            );
        }
    }
}
