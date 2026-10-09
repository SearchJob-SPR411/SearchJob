using DAL.data;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class FavoriteVacancyRepository : IFavoriteVacancyRepository
    {
        private readonly AppDbContext _context;

        public FavoriteVacancyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FavoriteVacancyEntity>> GetByUserIdAsync(int userId)
        {
            return await _context.FavoriteVacancies
                .Include(f => f.Vacancy)
                    .ThenInclude(v => v!.Company)
                .AsNoTracking()
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<FavoriteVacancyEntity?> GetByUserAndVacancyAsync(int userId, int vacancyId)
        {
            return await _context.FavoriteVacancies
                .FirstOrDefaultAsync(f => f.UserId == userId && f.VacancyId == vacancyId);
        }

        public async Task<FavoriteVacancyEntity> AddAsync(FavoriteVacancyEntity entity)
        {
            _context.FavoriteVacancies.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> RemoveAsync(int userId, int vacancyId)
        {
            var entity = await _context.FavoriteVacancies
                .FirstOrDefaultAsync(f => f.UserId == userId && f.VacancyId == vacancyId);

            if (entity == null)
            {
                return false;
            }

            _context.FavoriteVacancies.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsFavoriteAsync(int userId, int vacancyId)
        {
            return await _context.FavoriteVacancies
                .AnyAsync(f => f.UserId == userId && f.VacancyId == vacancyId);
        }

        public async Task<List<int>> GetFavoriteVacancyIdsAsync(int userId)
        {
            return await _context.FavoriteVacancies
                .Where(f => f.UserId == userId)
                .Select(f => f.VacancyId)
                .ToListAsync();
        }
    }
}
