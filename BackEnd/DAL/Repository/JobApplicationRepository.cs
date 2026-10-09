using DAL.data;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly AppDbContext _context;

        public JobApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobApplicationEntity>> GetByUserIdAsync(int userId)
        {
            return await _context.JobApplications
                .Include(a => a.Vacancy)
                    .ThenInclude(v => v!.Company)
                .Include(a => a.Resume)
                .AsNoTracking()
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();
        }

        public async Task<List<JobApplicationEntity>> GetByVacancyIdAsync(int vacancyId)
        {
            return await _context.JobApplications
                .Include(a => a.User)
                .Include(a => a.Resume)
                .Include(a => a.Vacancy)
                .AsNoTracking()
                .Where(a => a.VacancyId == vacancyId)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();
        }

        public async Task<JobApplicationEntity?> GetByIdAsync(int id)
        {
            return await _context.JobApplications
                .Include(a => a.User)
                .Include(a => a.Resume)
                .Include(a => a.Vacancy)
                    .ThenInclude(v => v!.Company)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<JobApplicationEntity> CreateAsync(JobApplicationEntity entity)
        {
            _context.JobApplications.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(JobApplicationEntity entity)
        {
            _context.JobApplications.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int userId, int vacancyId)
        {
            return await _context.JobApplications
                .AnyAsync(a => a.UserId == userId && a.VacancyId == vacancyId);
        }
    }
}
