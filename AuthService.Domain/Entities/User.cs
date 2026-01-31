using AuthService.Domain.Common;
using AuthService.Domain.Enums;
using AuthService.Domain.Events;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Models;
using System.Text.RegularExpressions;

namespace AuthService.Domain.Entities
{
    public class User : AggregateRoot
    {
        public string Username { get; private set; }
        public Email Email { get; private set;  }
        public PasswordHash PasswordHash { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public Role Role { get; private set; }
        public UserStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? LastLogin { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public int FailedLoginCount { get; private set; }

        public bool IsDeleted => DeletedAt.HasValue;
        public bool IsLocked => FailedLoginCount >= 5;

        // Parameterless constructor for EF Core
        private User() { }

        public User(string username, Email email, PasswordHash passwordHash, Role role, string firstname, string lastname)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            Firstname = firstname;
            Lastname = lastname;
            Status = UserStatus.Active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            FailedLoginCount = 0;
        }

        public static User Create(string username,string email,string passwordHash,string role, string firstname, string lastname)
        {
            var user = new User(
                username,
                Email.Create(email),
                PasswordHash.Create(passwordHash),
                Role.Create(role),
                firstname,
                lastname
            );

            user.AddDomainEvent(new UserRegisteredEvent(user.Id.ToString()));
            return user;
        }

        public void ChangePassword(string newHash)
        {
            PasswordHash = PasswordHash.Create(newHash);
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            if (IsDeleted)
                throw new DomainException("User already deleted");
            Status = UserStatus.Deleted;
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkLogin()
        {
            LastLogin = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void IncrementFailedLogin()
        {
            FailedLoginCount++;
        }

        public void ResetFailedLogin()
        {
            FailedLoginCount = 0;
        }

        public void Lock()
        {
            FailedLoginCount = 5;
        }

        public void UpdateUsername(string username)
        {
            Username = username;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLastLogin()
        {
            LastLogin = DateTime.UtcNow;
        }

        public static void ValidatePassword(string password)
        {
            if(password.Length < 8)
                throw new DomainException("Password must be at least 8 characters long");
            if(!password.Any(char.IsDigit))
                throw new DomainException("Password must contain at least one digit");
            if (!password.Any(char.IsUpper))
                throw new DomainException("Password must contain at least one uppercase letter");
            if (!password.Any(char.IsLower))
                throw new DomainException("Password must contain at least one lowercase letter");
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_\-+=\[{\]};:'"",<.>/?\\|`~]"))
                throw new DomainException("Password must contain at least one special character");
        }
    }
}
