using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public string Token { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        public bool IsActive => RevokedAt is null && DateTime.UtcNow < ExpiresAt;

        public RefreshToken(string token, Guid userId, DateTime expiresAt)
        {
            Id = Guid.NewGuid();
            Token = token;
            UserId = userId;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }

        public void Revoke()
        {
            RevokedAt = DateTime.UtcNow;
        }
    }
}
