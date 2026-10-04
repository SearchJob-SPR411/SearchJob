using DAL.data;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly AppDbContext _context;
        public ResumeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ResumeEntity>> GetAllAsync()
        {
            return await _context.Resumes
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<ResumeEntity?> GetByIdAsync(int id)
        {
            return await _context.Resumes
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<IEnumerable<ResumeEntity>> GetByUserIdAsync(int userId)
        {
            return await _context.Resumes
                .AsNoTracking()
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
        public async Task<int> CountByUserIdAsync(int userId)
        {
            return await _context.Resumes
                .CountAsync(r => r.UserId == userId);
        }
        public async Task<ResumeEntity> CreateAsync(ResumeEntity resume)
        {
            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();

            return resume;
        }
        public async Task UpdateAsync(ResumeEntity resume)
        {
            _context.Resumes.Update(resume);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(ResumeEntity resume)
        {
            _context.Resumes.Remove(resume);
            await _context.SaveChangesAsync();
        }
    }
}
