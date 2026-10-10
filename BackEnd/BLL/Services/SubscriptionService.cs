using AutoMapper;
using BLL.DTO;
using DAL.Entity;
using DAL.Enums;
using DAL.Repository;

namespace BLL.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _repository;
        private readonly IMapper _mapper;

        public SubscriptionService(ISubscriptionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SubscriptionDto?> GetByUserIdAsync(int userId)
        {
            var subscription = await _repository.GetByUserIdAsync(userId);

            if (subscription == null)
            {
                return null;
            }

            return _mapper.Map<SubscriptionDto>(subscription);
        }

        public async Task<bool> ActivatePremiumAsync(int userId, DateTime expiresAt)
        {
            var subscription = await _repository.GetByUserIdAsync(userId);

            if (subscription == null)
            {
                return false;
            }

            subscription.Type = SubscriptionType.Premium;
            subscription.Status = SubscriptionStatus.Active;
            subscription.ExpiresAt = expiresAt;

            await _repository.UpdateAsync(subscription);

            return true;
        }

        public bool IsPremiumActive(SubscriptionDto subscription)
        {
            if (subscription.Type != SubscriptionType.Premium)
            {
                return false;
            }

            if (subscription.Status != SubscriptionStatus.Active)
            {
                return false;
            }

            if (subscription.ExpiresAt.HasValue &&
                subscription.ExpiresAt.Value <= DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }
    }
}