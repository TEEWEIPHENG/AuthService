using AuthService.Domain.Exceptions;

namespace AuthService.Domain.Models
{
    public sealed record Role
    {
        public string Value { get; init; }

        private Role(string value) => Value = value;
        public static Role Create(string value)
        {
            if (value is not ("Admin" or "User"))
                throw new DomainException("Invalid role");

            return new Role(value);
        }
    }
}
