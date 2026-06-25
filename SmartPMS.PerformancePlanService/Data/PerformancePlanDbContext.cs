using Microsoft.EntityFrameworkCore;
using SmartPMS.PerformancePlanService.Models;

namespace SmartPMS.PerformancePlanService.Data;

public class PerformancePlanDbContext : DbContext
{
    public PerformancePlanDbContext(DbContextOptions<PerformancePlanDbContext> options)
        : base(options)
    {
    }

    public DbSet<PerformancePlan> PerformancePlans => Set<PerformancePlan>();

    public DbSet<PerformancePlanItem> PerformancePlanItems => Set<PerformancePlanItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PerformancePlan>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EmployeeCode)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.EmployeeName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.FinancialYear)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(x => new { x.EmployeeId, x.FinancialYear });

            entity.HasMany(x => x.Items)
                .WithOne(x => x.PerformancePlan)
                .HasForeignKey(x => x.PerformancePlanId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PerformancePlanItem>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.KraTitle)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.KpiDescription)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(x => x.Target)
                .HasMaxLength(500);

            entity.Property(x => x.MeasurementCriteria)
                .HasMaxLength(500);

            entity.Property(x => x.Weightage)
                .HasColumnType("decimal(18,2)");
        });
    }
}