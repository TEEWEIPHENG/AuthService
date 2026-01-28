using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.Commands
{
    public sealed class LoginCommand
    {
        public string Username { get; init; } = default!;
        public string Password { get; init; } = default!;
    }
}
