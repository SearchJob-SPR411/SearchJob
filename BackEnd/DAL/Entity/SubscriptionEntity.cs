using System.ComponentModel.DataAnnotations;

namespace DAL.Entity
{
    public enum SubscriptionType
    {
        Free,
        Premium
    }

    public enum SubscriptionStatus
    {
        Active,
        Expired,
        Cancelled
    }

    public class SubscriptionEntity : IBaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public UserEntity? User { get; set; }

        public SubscriptionType Type { get; set; }

        public SubscriptionStatus Status { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}
