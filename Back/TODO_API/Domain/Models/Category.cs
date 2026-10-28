using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Color { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public Category(string name, string color, Guid userId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Color = color;
            UserId = userId;
            DateTime now = DateTime.Now;
            CreatedAt = now;
            LastUpdatedAt = now;
        }

        public void Update(string name, string color)
        {
            Name = name;
            Color = color;
            LastUpdatedAt = DateTime.Now;
        }
    }
}
