using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Domain.Models
{
    public class TaskItem
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime Due_date { get; private set; }
        public bool IsCompleted { get; private set; }
        public Guid CategoryId { get; private set;  }
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }

        public TaskItem(string name, string description, DateTime due_date, bool isCompleted, Guid categoryId, Guid userId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Due_date = due_date;
            IsCompleted = isCompleted;
            CategoryId = categoryId;
            UserId = userId;
            DateTime now = DateTime.Now;
            CreatedAt = now;
            LastUpdatedAt = now;
        }

        public void Update(string name, string description, DateTime due_date, bool isCompleted, Guid categoryId)
        {
            Name = name;
            Description = description;
            Due_date = due_date;
            IsCompleted = isCompleted;
            CategoryId = categoryId;
            LastUpdatedAt = DateTime.Now;
        }

    }
}
