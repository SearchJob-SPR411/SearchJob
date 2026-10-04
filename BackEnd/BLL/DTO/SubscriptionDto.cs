using DAL.Entity;

namespace BLL.DTO
{
    public class SubscriptionDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public SubscriptionType Type { get; set; }

        public SubscriptionStatus Status { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}