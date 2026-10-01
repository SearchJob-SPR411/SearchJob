using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Entity;

namespace DAL.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<VacancyEntity> Vacancies => Set<VacancyEntity>();
        public DbSet<ResumeEntity> Resumes => Set<ResumeEntity>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.LastName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(256).IsRequired();
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.PhoneNumber).HasMaxLength(30);
                entity.Property(u => u.CreatedAt).IsRequired();

                entity.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<VacancyEntity>(entity =>
            {
                entity.Property(v => v.Title)
                .HasMaxLength(150)
                .IsRequired();

                entity.Property(v => v.CompanyName)
                .HasMaxLength(150)
                .IsRequired();

                entity.Property(v => v.Location)
                .HasMaxLength(150)
                .IsRequired();

                entity.Property(v => v.EmploymentType)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(v => v.WorkFormat)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(v => v.SalaryMin)
                    .HasColumnType("decimal(10,2)");

                entity.Property(v => v.SalaryMax)
                    .HasColumnType("decimal(10,2)");

                entity.Property(v => v.Description)
                    .IsRequired();

                entity.Property(v => v.IsActive)
                    .IsRequired();

                entity.Property(v => v.PostedAt)
                    .IsRequired();

                entity.Property(v => v.UpdatedAt);

                entity.HasOne(v => v.User)
                    .WithMany(u => u.Vacancies)
                    .HasForeignKey(v => v.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ResumeEntity>(entity =>
            {
                entity.Property(r => r.Title)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(r => r.Summary)
                    .HasMaxLength(2000);

                entity.Property(r => r.Skills)
                    .HasMaxLength(2000);

                entity.Property(r => r.Experience)
                    .HasMaxLength(5000);

                entity.Property(r => r.Education)
                    .HasMaxLength(5000);

                entity.Property(r => r.CreatedAt)
                    .IsRequired();

                entity.Property(r => r.UpdatedAt);

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Resumes)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
