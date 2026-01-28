using AuthService.Domain.Common;
using AuthService.Domain.Enums;
using AuthService.Domain.Events;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Models;

namespace AuthService.Domain.Entities
{
    public class User : AggregateRoot
    {
        private const int MaxFailedAttempts = 5;

        public string Username { get; private set; }
        public Email Email { get; private set;  }
        public PasswordHash PasswordHash { get; private set; }
        public Role Role { get; private set; }
        public UserStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? LastLogin { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public int FailedLoginAttempts { get; private set; }
        public DateTime? LockedAt { get; private set; }

        public bool IsDeleted => DeletedAt.HasValue;
        public bool IsLocked => LockedAt.HasValue && LockedAt > DateTime.UtcNow;

        private User() { } //For ORM

        public User(string username, Email email, PasswordHash passwordHash, Role role)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            Status = UserStatus.Active;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public static User Create(string username,string email,string passwordHash,string role)
        {
            var user = new User(
                username,
                Email.Create(email),
                PasswordHash.Create(passwordHash),
                Role.From(role)
            );

            user.AddDomainEvent(new UserRegisteredEvent(user.Id.ToString()));
            return user;
        }

        public void ChangePassword(string newHash)
        {
            PasswordHash = PasswordHash.Create(newHash);
            //unlock
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

        public void RecordFailedLogin()
        {
            FailedLoginAttempts++;

            if (FailedLoginAttempts >= MaxFailedAttempts)
            {
                LockedAt = DateTime.UtcNow;
                Status = UserStatus.Locked;
            }

            UpdatedAt = DateTime.UtcNow;
        }

        public void RecordSuccessfulLogin()
        {
            FailedLoginAttempts = 0;
            LockedAt = null;
            Status = UserStatus.Active;
            LastLogin = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        public void EnsureNotLocked()
        {
            if (IsLocked)
                throw new DomainException($"Account locked until {LockedAt:O}");
        }

    }
}
