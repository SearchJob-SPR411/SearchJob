using BLL.DTO;

namespace BLL.Services
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDto?> GetByUserIdAsync(int userId);
        Task<bool> ActivatePremiumAsync(int userId, DateTime expiresAt);
        bool IsPremiumActive(SubscriptionDto subscription);
    }
}