using Microsoft.EntityFrameworkCore;
using SmartPMS.SelfAppraisalService.Models;

namespace SmartPMS.SelfAppraisalService.Data;

public class SelfAppraisalDbContext : DbContext
{
    public SelfAppraisalDbContext(DbContextOptions<SelfAppraisalDbContext> options)
        : base(options)
    {
    }

    public DbSet<SelfAppraisal> SelfAppraisals => Set<SelfAppraisal>();
    public DbSet<SelfAppraisalItem> SelfAppraisalItems => Set<SelfAppraisalItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SelfAppraisal>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EmployeeCode).HasMaxLength(50).IsRequired();
            entity.Property(x => x.EmployeeName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.FinancialYear).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Status).HasMaxLength(50).IsRequired();

            entity.HasIndex(x => x.PerformancePlanId)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            entity.HasIndex(x => new { x.EmployeeId, x.FinancialYear });

            entity.HasMany(x => x.Items)
                .WithOne(x => x.SelfAppraisal)
                .HasForeignKey(x => x.SelfAppraisalId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SelfAppraisalItem>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.KraTitle).HasMaxLength(200).IsRequired();
            entity.Property(x => x.KpiDescription).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.Target).HasMaxLength(500);
            entity.Property(x => x.Achievement).HasMaxLength(2000);
            entity.Property(x => x.EmployeeComments).HasMaxLength(2000);
            entity.Property(x => x.Weightage).HasColumnType("decimal(18,2)");
            entity.Property(x => x.SelfRating).HasColumnType("decimal(5,2)");

            entity.HasIndex(x => new { x.SelfAppraisalId, x.PerformancePlanItemId })
                .IsUnique();
        });
    }
}
