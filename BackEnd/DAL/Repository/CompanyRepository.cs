using DAL.data;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompanyEntity>> GetAllAsync()
        {
            return await _context.Companies
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CompanyEntity?> GetByIdAsync(int id)
        {
            return await _context.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<CompanyEntity>> GetByUserIdAsync(int userId)
        {
            return await _context.Companies
                .AsNoTracking()
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<CompanyEntity> CreateAsync(CompanyEntity company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return company;
        }

        public async Task UpdateAsync(CompanyEntity company)
        {
            _context.Companies.Update(company);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CompanyEntity company)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
    }
}