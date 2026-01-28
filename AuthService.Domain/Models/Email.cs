using AuthService.Domain.Exceptions;

namespace AuthService.Domain.Models
{
    public sealed record Email
    {
        public string Value { get; init; }

        private Email(string value) => Value = value;

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
                throw new DomainException("Invalid email address");

            return new Email(value.ToLowerInvariant());
        }
    }
}
