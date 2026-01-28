using AuthService.Domain.Exceptions;

namespace AuthService.Domain.Models
{
    public sealed record PasswordHash
    {
        public string Value { get; init; }

        private PasswordHash(string value) => Value = value;

        public static PasswordHash Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Password hash cannot be empty");

            return new PasswordHash(value);
        }
    }
}
