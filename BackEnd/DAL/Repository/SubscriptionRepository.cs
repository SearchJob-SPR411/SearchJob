using DAL.data;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _context;

        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SubscriptionEntity?> GetByUserIdAsync(
            int userId)
        {
            return await _context.Subscriptions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<SubscriptionEntity> CreateAsync(
            SubscriptionEntity subscription)
        {
            _context.Subscriptions.Add(subscription);

            await _context.SaveChangesAsync();

            return subscription;
        }

        public async Task UpdateAsync(
            SubscriptionEntity subscription)
        {
            _context.Subscriptions.Update(subscription);

            await _context.SaveChangesAsync();
        }
    }
}