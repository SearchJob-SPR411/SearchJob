using DAL.data;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly AppDbContext _context;

        public VacancyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<VacancyEntity>> GetAllAsync()
        {
            return await _context.Vacancies
                .Include(v => v.Company)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<VacancyEntity?> GetByIdAsync(int id)
        {
            return await _context.Vacancies
                .Include(v => v.Company)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<VacancyEntity>> GetByCompanyIdAsync(int companyId)
        {
            return await _context.Vacancies
                .Include(v => v.Company)
                .AsNoTracking()
                .Where(v => v.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<VacancyEntity> CreateAsync(VacancyEntity vacancy)
        {
            _context.Vacancies.Add(vacancy);

            await _context.SaveChangesAsync();

            return vacancy;
        }

        public async Task UpdateAsync(VacancyEntity vacancy)
        {
            _context.Vacancies.Update(vacancy);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(VacancyEntity vacancy)
        {
            _context.Vacancies.Remove(vacancy);

            await _context.SaveChangesAsync();
        }
    }
}