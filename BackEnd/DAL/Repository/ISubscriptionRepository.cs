using DAL.Entity;

namespace DAL.Repository
{
    public interface ISubscriptionRepository
    {
        Task<SubscriptionEntity?> GetByUserIdAsync(int userId);

        Task<SubscriptionEntity> CreateAsync(SubscriptionEntity subscription);

        Task UpdateAsync(SubscriptionEntity subscription);
    }
}