using System;

namespace EventManagementSystem.Entity
{
    public class UserEntity
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public int FailedAttempts { get; set; }
        public bool IsLocked { get; set; }

        public string ResetToken { get; set; }
        public DateTime? TokenExpiry { get; set; }
    }
}