using System;

namespace SmartMedical.Core.Entities.Auth
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Revoked { get; set; }
        public string RevokedReason { get; set; }

        // Navigation properties
        public User User { get; set; }
    }
}
