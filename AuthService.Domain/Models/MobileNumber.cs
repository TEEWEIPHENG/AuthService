using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Domain.Exceptions;

namespace AuthService.Domain.Models
{
    public class PhoneNumber
    {
        public string Value { get; init; }

        private PhoneNumber(string value) => Value = value;

        public static PhoneNumber Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains('+'))
                throw new DomainException("Invalid Mobile Number");

            return new PhoneNumber(value.ToLowerInvariant());
        }
    }
}