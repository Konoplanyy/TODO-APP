using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }

        public User(string username, string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            DateTime now = DateTime.Now;
            CreatedAt = now;
            LastUpdatedAt = now;
        }

        public void Update(string username, string email)
        {
            Username = username;
            Email = email;
            LastUpdatedAt = DateTime.Now;
        }

        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
            LastUpdatedAt = DateTime.Now;
        }
    }
}
