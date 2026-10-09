using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<VacancyEntity> Vacancies => Set<VacancyEntity>();
        public DbSet<ResumeEntity> Resumes => Set<ResumeEntity>();
        public DbSet<CompanyEntity> Companies => Set<CompanyEntity>();
        public DbSet<FavoriteVacancyEntity> FavoriteVacancies => Set<FavoriteVacancyEntity>();
        public DbSet<JobApplicationEntity> JobApplications => Set<JobApplicationEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.Property(u => u.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(u => u.LastName)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(u => u.Email)
                    .HasMaxLength(256)
                    .IsRequired();

                entity.Property(u => u.PasswordHash)
                    .IsRequired();

                entity.Property(u => u.PhoneNumber)
                    .HasMaxLength(30);

                entity.Property(u => u.Role)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(u => u.CreatedAt)
                    .IsRequired();

                entity.HasIndex(u => u.Email)
                    .IsUnique();
            });

            modelBuilder.Entity<VacancyEntity>(entity =>
            {
                entity.Property(v => v.Title)
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

                entity.Property(v => v.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
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

            modelBuilder.Entity<CompanyEntity>(entity =>
            {
                entity.Property(c => c.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(c => c.Description)
                    .HasMaxLength(2000);

                entity.Property(c => c.Website)
                    .HasMaxLength(500);

                entity.Property(c => c.Location)
                    .HasMaxLength(150);

                entity.Property(c => c.LogoUrl)
                    .HasMaxLength(500);

                entity.Property(c => c.CreatedAt)
                    .IsRequired();

                entity.HasOne(c => c.User)
                    .WithMany()
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.Vacancies)
                    .WithOne(v => v.Company)
                    .HasForeignKey(v => v.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<FavoriteVacancyEntity>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.HasIndex(f => new { f.UserId, f.VacancyId })
                    .IsUnique();

                entity.Property(f => f.CreatedAt)
                    .IsRequired();

                entity.HasOne(f => f.User)
                    .WithMany(u => u.FavoriteVacancies)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.Vacancy)
                    .WithMany(v => v.Favorites)
                    .HasForeignKey(f => f.VacancyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<JobApplicationEntity>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.CoverLetter)
                    .HasMaxLength(2000);

                entity.Property(a => a.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(a => a.AppliedAt)
                    .IsRequired();

                entity.Property(a => a.UpdatedAt);

                entity.HasOne(a => a.User)
                    .WithMany(u => u.Applications)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Vacancy)
                    .WithMany(v => v.Applications)
                    .HasForeignKey(a => a.VacancyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Resume)
                    .WithMany()
                    .HasForeignKey(a => a.ResumeId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}