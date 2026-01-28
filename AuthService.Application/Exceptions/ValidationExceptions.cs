using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Exceptions
{
    public class ValidationExceptions : Exception
    {
        public ValidationExceptions() { }
        public ValidationExceptions(string message)
            : base(message) { }
        public ValidationExceptions(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
