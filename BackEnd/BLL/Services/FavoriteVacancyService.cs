using AutoMapper;
using BLL.DTO;
using DAL.Entity;
using DAL.Repository;

namespace BLL.Services
{
    public class FavoriteVacancyService : IFavoriteVacancyService
    {
        private readonly IFavoriteVacancyRepository _favoriteRepository;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly IMapper _mapper;

        public FavoriteVacancyService(
            IFavoriteVacancyRepository favoriteRepository,
            IVacancyRepository vacancyRepository,
            IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _vacancyRepository = vacancyRepository;
            _mapper = mapper;
        }

        public async Task<List<VacancyDto>> GetFavoritesAsync(int userId)
        {
            var favorites = await _favoriteRepository.GetByUserIdAsync(userId);
            var vacancies = favorites
                .Where(f => f.Vacancy != null)
                .Select(f => f.Vacancy!)
                .ToList();

            return _mapper.Map<List<VacancyDto>>(vacancies);
        }

        public async Task<bool> AddToFavoritesAsync(int userId, int vacancyId)
        {
            var vacancy = await _vacancyRepository.GetByIdAsync(vacancyId);
            if (vacancy == null)
            {
                return false;
            }

            var exists = await _favoriteRepository.IsFavoriteAsync(userId, vacancyId);
            if (exists)
            {
                return true;
            }

            var entity = new FavoriteVacancyEntity
            {
                UserId = userId,
                VacancyId = vacancyId,
                CreatedAt = DateTime.UtcNow
            };

            await _favoriteRepository.AddAsync(entity);
            return true;
        }

        public async Task<bool> RemoveFromFavoritesAsync(int userId, int vacancyId)
        {
            return await _favoriteRepository.RemoveAsync(userId, vacancyId);
        }

        public async Task<bool> IsFavoriteAsync(int userId, int vacancyId)
        {
            return await _favoriteRepository.IsFavoriteAsync(userId, vacancyId);
        }

        public async Task<List<int>> GetFavoriteVacancyIdsAsync(int userId)
        {
            return await _favoriteRepository.GetFavoriteVacancyIdsAsync(userId);
        }
    }
}
