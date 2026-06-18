using Microsoft.EntityFrameworkCore;
using SmartPMS.EmployeeService.Models;

namespace SmartPMS.EmployeeService.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.EmployeeCode)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.HasIndex(x => x.EmployeeCode)
                    .IsUnique();

                entity.Property(x => x.FullName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.Email)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.HasIndex(x => x.Email)
                    .IsUnique();

                entity.Property(x => x.Department)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Designation)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Level)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.IsPaperPMS)
                    .HasDefaultValue(false);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.IsDeleted)
                    .HasDefaultValue(false);
            });
        }
    }
}
